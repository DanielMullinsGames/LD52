using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesArea : ManagedBehaviour
{
    public Civilization civ;
    public Tooltip tooltip;

    [SerializeField]
    private GameObject resourcePanelPrefab;

    [SerializeField]
    private float panelsSpacing;

    [SerializeField]
    private Transform panelsAnchor;

    private List<ResourcePanel> panels = new List<ResourcePanel>();

    public bool CanAffordCost(List<ResourceType> costTypes, List<float> costAmounts)
    {
        for (int i = 0; i < costTypes.Count; i++)
        {
            if (GetResource(costTypes[i]) == null || GetResource(costTypes[i]).Amount < costAmounts[i])
            {
                return false;
            }
        }
        return true;
    }

    public void TickResources(float timeStep)
    {
        panels.ForEach(x => x.TickResource(timeStep));
    }

    public ResourcePanel GetOrCreateResource(ResourceType resourceType)
    {
        var panel = GetResource(resourceType);
        if (panel == null)
        {
            return AddResourcePanel(resourceType);
        }
        else
        {
            return panel;
        }
    }

    public ResourcePanel GetResource(ResourceType resourceType)
    {
        return panels.Find(x => x.Data.resourceType == resourceType);
    }

    public ResourcePanel AddResourcePanel(ResourceType resourceType)
    {
        var panelObj = Instantiate(resourcePanelPrefab, transform);
        panelObj.transform.position = panelsAnchor.position + Vector3.down * panelsSpacing * panels.Count;

        var panel = panelObj.GetComponent<ResourcePanel>();
        panel.Initialize(GameDataReferences.GetResourceData(resourceType), this);

        panels.Add(panel);
        return panel;
    }

    public void ShowCannotAfford(List<ResourceType> costTypes, List<float> costAmounts)
    {
        for (int i = 0; i < costTypes.Count; i++)
        {
            var resPanel = GetResource(costTypes[i]);
            if (resPanel != null && resPanel.Amount < costAmounts[i])
            {
                resPanel.ShowCannotAfford();
            }
        }
    }

    public override void ManagedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F))
        {
            TickResources(10f);
        }
    }
}

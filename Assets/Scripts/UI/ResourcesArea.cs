using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesArea : ManagedBehaviour
{
    [SerializeField]
    private GameObject resourcePanelPrefab;

    [SerializeField]
    private float panelsSpacing;

    [SerializeField]
    private Transform panelsAnchor;

    public List<ResourcePanel> Panels => panels;
    private List<ResourcePanel> panels = new List<ResourcePanel>();

    public ResourcePanel AddResourcePanel(ResourceData data)
    {
        var panelObj = Instantiate(resourcePanelPrefab);
        panelObj.transform.position = panelsAnchor.position + Vector3.down * panelsSpacing * panels.Count;

        var panel = panelObj.GetComponent<ResourcePanel>();
        panel.Initialize(data);

        panels.Add(panel);
        return panel;
    }
}

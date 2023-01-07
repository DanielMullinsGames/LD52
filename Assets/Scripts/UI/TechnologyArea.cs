using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyArea : ManagedBehaviour
{
    [SerializeField]
    private Transform availableTechnogiesParent;

    [SerializeField]
    private Transform learnedTechnologiesParent;

    [SerializeField]
    private GameObject availablePanelPrefab;

    [SerializeField]
    private GameObject learnedPanelPrefab;

    [SerializeField]
    private Transform availablePosAnchor;

    [SerializeField]
    private float availableSpacing;

    [SerializeField]
    private Transform learnedPosAnchor;

    [SerializeField]
    private float learnedSpacing;

    private List<TechnologyPanel> learnedPanels = new List<TechnologyPanel>();
    private List<TechnologyPurchasePanel> availablePanels = new List<TechnologyPurchasePanel>();

    public TechnologyPurchasePanel AddTechnologyToAvailable(TechnologyType techType)
    {
        var panelObj = Instantiate(availablePanelPrefab, availableTechnogiesParent);

        var panel = panelObj.GetComponent<TechnologyPurchasePanel>();
        panel.Initialize(GameDataReferences.GetTechnologyData(techType));

        availablePanels.Add(panel);

        UpdatePanelPositions();
        return panel;
    }

    public TechnologyPanel AddTechnologyToLearned(TechnologyType techType)
    {
        var panelObj = Instantiate(learnedPanelPrefab, learnedTechnologiesParent);

        var panel = panelObj.GetComponent<TechnologyPanel>();
        panel.Initialize(GameDataReferences.GetTechnologyData(techType));

        learnedPanels.Add(panel);

        UpdatePanelPositions();
        return panel;
    }

    public void RemoveTechnologyFromAvailable(TechnologyType type)
    {
        UpdatePanelPositions();
    }

    public void PurchaseTechnology(TechnologyType type)
    {
        RemoveTechnologyFromAvailable(type);
        AddTechnologyToLearned(type);
    }

    private void UpdatePanelPositions()
    {
        for (int i = 0; i < learnedPanels.Count; i++)
        {
            float y = learnedPosAnchor.localPosition.y - learnedSpacing * i;
            learnedPanels[i].transform.localPosition = new Vector2(learnedPosAnchor.localPosition.x, y);
        }

        for (int i = 0; i < availablePanels.Count; i++)
        {
            float y = availablePosAnchor.localPosition.y - availableSpacing * i;
            availablePanels[i].transform.localPosition = new Vector2(learnedPosAnchor.localPosition.x, y);
        }
    }
}

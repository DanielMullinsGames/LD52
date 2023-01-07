using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyArea : ManagedBehaviour
{
    [SerializeField]
    private Civilization civ;

    [SerializeField]
    private TechnologyActivationManager activationManager;

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
        panel.CursorSelectStarted += (Interactable p) => OnAttemptPurchase(p.GetComponent<TechnologyPurchasePanel>());

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

    public void RemoveTechnologyFromAvailable(TechnologyPurchasePanel panel)
    {
        availablePanels.Remove(panel);
        Destroy(panel.gameObject);
        UpdatePanelPositions();
    }

    private void OnAttemptPurchase(TechnologyPurchasePanel panel)
    {
        if (civ.Resources.CanAffordCost(panel.Data.costTypes, panel.Data.costAmounts))
        {
            PurchaseTechnology(panel);
        }
        else
        {
            // TODO show cannot afford
        }
    }

    private void PurchaseTechnology(TechnologyPurchasePanel panel)
    {
        activationManager.ActivateTech(panel);
        RemoveTechnologyFromAvailable(panel);
        AddTechnologyToLearned(panel.Data.technologyType);
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

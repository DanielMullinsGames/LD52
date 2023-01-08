using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class TechnologyArea : ManagedBehaviour
{
    public Tooltip tooltip;

    private List<TechnologyData> learnedTech = new List<TechnologyData>();
    private List<TechnologyData> availableTech = new List<TechnologyData>();

    public Civilization civ;

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

    private void Start()
    {
        UpdateAvailableTech();
    }

    public bool Learned(TechnologyType type)
    {
        return learnedTech.Exists(x => x.technologyType == type);
    }

    public TechnologyPurchasePanel AddTechnologyToAvailable(TechnologyType techType)
    {
        var panelObj = Instantiate(availablePanelPrefab, availableTechnogiesParent);

        var panel = panelObj.GetComponent<TechnologyPurchasePanel>();
        panel.Initialize(GameDataReferences.GetTechnologyData(techType), this);
        panel.CursorSelectStarted += (Interactable p) => OnAttemptPurchase(p.GetComponent<TechnologyPurchasePanel>());

        availablePanels.Add(panel);

        UpdatePanelPositions();
        return panel;
    }

    public TechnologyPanel AddTechnologyToLearned(TechnologyType techType)
    {
        var panelObj = Instantiate(learnedPanelPrefab, learnedTechnologiesParent);

        var panel = panelObj.GetComponent<TechnologyPanel>();
        panel.Initialize(GameDataReferences.GetTechnologyData(techType), this);

        learnedPanels.Add(panel);

        UpdatePanelPositions();
        return panel;
    }

    private void RemoveTechnologyFromAvailable(TechnologyPurchasePanel panel)
    {
        availablePanels.Remove(panel);
        Destroy(panel.gameObject);
        UpdatePanelPositions();
    }

    private void UpdateAvailableTech()
    {
        var allTech = GameDataReferences.GetAllTechnology();
        foreach (var tech in allTech)
        {
            if (availableTech.Count < 7 && (tech.civPrereq == CivType.None || tech.civPrereq == civ.type)
                && !availableTech.Contains(tech) && !learnedTech.Contains(tech) 
                && !tech.prerequisites.Exists(x => !learnedTech.Contains(x)))
            {
                if (civ.type == CivType.Planet && tech.technologyType == TechnologyType.UnlockGrain)
                {
                    return;
                }

                availableTech.Add(tech);
                AddTechnologyToAvailable(tech.technologyType);
            }
        }
    }

    private void OnAttemptPurchase(TechnologyPurchasePanel panel)
    {
        if (civ.Resources.CanAffordCost(panel.Data.costTypes, panel.Data.costAmounts))
        {
            PurchaseTechnology(panel);
        }
        else
        {
            civ.Resources.ShowCannotAfford(panel.Data.costTypes, panel.Data.costAmounts);

            Tween.Shake(panel.anim, Vector2.zero, new Vector2(0.02f, 0.02f), 0.25f, 0f);
            AudioController.Instance.PlaySound2D("negate", 1f, 0f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
        }
    }

    private void PurchaseTechnology(TechnologyPurchasePanel panel)
    {
        AudioController.Instance.PlaySound2D("purchase_technology", 1f, 0f, pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
        activationManager.ActivateTech(panel);

        RemoveTechnologyFromAvailable(panel);
        AddTechnologyToLearned(panel.Data.technologyType);

        availableTech.Remove(panel.Data);
        learnedTech.Add(panel.Data);

        UpdateAvailableTech();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class PoliciesArea : ManagedBehaviour
{
    public Tooltip tooltip;

    [SerializeField]
    private Civilization civ = default;

    [SerializeField]
    private PolicyActivationManager activationManager = default;

    [SerializeField]
    private GameObject policyPanelPrefab = default;

    [SerializeField]
    private float panelsSpacing = default;

    [SerializeField]
    private Transform panelsAnchor = default;

    public List<PolicyPanel> Panels => panels;
    private List<PolicyPanel> panels = new List<PolicyPanel>();

    public void TickCooldowns(float timeStep)
    {
        panels.ForEach(x => x.TickCooldown(timeStep));
    }

    public PolicyPanel AddPolicyPanel(PolicyType type)
    {
        var data = GameDataReferences.GetPolicyData(type);

        var panelObj = Instantiate(policyPanelPrefab, transform);

        Vector2 pos = panelsAnchor.position + Vector3.right * panelsSpacing * panels.Count;
        panelObj.transform.position = pos + Vector2.down * 1f;
        Tween.Position(panelObj.transform, pos, 0.25f, 0f, Tween.EaseOut);

        var panel = panelObj.GetComponent<PolicyPanel>();
        panel.Initialize(data, civ);

        panels.Add(panel);
        panel.SetHotkey(panels.Count);
        panel.Activated += OnPanelActivated;
        return panel;
    }

    private void OnPanelActivated(PolicyPanel panel)
    {
        if (civ.Resources.CanAffordCost(panel.Data.costTypes, panel.Data.costAmounts))
        {
            activationManager.ActivatePolicy(panel);
        }
        else
        {
            //TODO show cannot afford
        }
    }
}

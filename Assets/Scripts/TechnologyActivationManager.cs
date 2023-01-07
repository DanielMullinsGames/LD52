using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyActivationManager : ManagedBehaviour
{
    [SerializeField]
    private Civilization civ = default;

    public void ActivateTech(TechnologyPurchasePanel panel)
    {
        // Pay cost
        for (int i = 0; i < panel.Data.costTypes.Count; i++)
        {
            civ.Resources.GetOrCreateResource(panel.Data.costTypes[i]).PayCost(panel.Data.costAmounts[i]);
        }

        // Standard effects
        if (panel.Data.policyUnlock != null)
        {
            civ.Policies.AddPolicyPanel(panel.Data.policyUnlock.policyType);
        }

        if (panel.Data.resourceModification != ResourceType.None)
        {
            civ.Resources.GetOrCreateResource(panel.Data.resourceModification).Maximum += panel.Data.resourceMaxModification;
            civ.Resources.GetOrCreateResource(panel.Data.resourceModification).Rate += panel.Data.resourceRateModification;
        }

        // Special effects
        switch (panel.Data.technologyType)
        {
            default:
                break;
        }
    }
}

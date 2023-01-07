using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyActivationManager : ManagedBehaviour
{
    [SerializeField]
    private Civilization civ = default;

    public void ActivatePolicy(PolicyPanel panel)
    {
        // Pay cost
        for (int i = 0; i < panel.Data.costTypes.Count; i++)
        {
            civ.Resources.GetOrCreateResource(panel.Data.costTypes[i]).PayCost(panel.Data.costAmounts[i]);
        }

        // Activate
        switch (panel.Data.policyType)
        {
            case PolicyType.BuildFarm:
                civ.AddImprovement(TileImprovementType.Farm);
                civ.Resources.GetOrCreateResource(ResourceType.Grain).Rate += 0.1f;
                break;
        }
    }
}

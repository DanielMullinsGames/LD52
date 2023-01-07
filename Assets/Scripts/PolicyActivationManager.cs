using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyActivationManager : ManagedBehaviour
{
    [SerializeField]
    private Civilization civ = default;

    public void ActivatePolicy(PolicyPanel panel)
    {
        //TODO: pay costs

        switch (panel.Data.policyType)
        {
            case PolicyType.BuildFarm:
                civ.AddImprovement(TileImprovementType.Farm);
                civ.Resources.GetOrCreateResource(ResourceType.Grain).Rate += 0.1f;
                break;
        }
    }
}

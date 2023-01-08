using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyActivationManager : ManagedBehaviour
{
    [SerializeField]
    private Civilization civ = default;

    private bool doubleNextEffect;

    public void ActivatePolicy(PolicyPanel panel, bool noCost = false)
    {
        if (doubleNextEffect)
        {
            doubleNextEffect = false;
            CustomCoroutine.WaitThenExecute(0.25f, () => ActivatePolicy(panel, noCost: true));
        }

        // Pay cost
        if (!noCost)
        {
            for (int i = 0; i < panel.Data.costTypes.Count; i++)
            {
                civ.Resources.GetOrCreateResource(panel.Data.costTypes[i]).PayCost(panel.Data.costAmounts[i]);
            }
        }

        // Basic effects
        if (panel.Data.resourceModification != ResourceType.None)
        {
            civ.Resources.GetOrCreateResource(panel.Data.resourceModification).Amount += panel.Data.resourceAmountModification;

            if (panel.Data.resourceAmountModification > 0f)
            {
                civ.Resources.GetResource(panel.Data.resourceModification).ShowGain(panel.Data.resourceAmountModification);
            }

            civ.Resources.GetOrCreateResource(panel.Data.resourceModification).Rate += panel.Data.resourceRateModification;
        }

        // Special effects
        switch (panel.Data.policyType)
        {
            case PolicyType.BuildFarm:
                civ.AddImprovement(TileImprovementType.Farm);

                if (civ.Technologies.Learned(TechnologyType.GrainRate1))
                {
                    civ.Resources.GetOrCreateResource(panel.Data.resourceModification).Rate += panel.Data.resourceRateModification;
                }
                break;
            case PolicyType.BuildChurch:
                civ.AddImprovement(TileImprovementType.Church);
                break;
            case PolicyType.BuildSchool:
                civ.AddImprovement(TileImprovementType.School);
                break;
            case PolicyType.War:
                civ.Resources.GetOrCreateResource(ResourceType.Money).Rate += 1;
                civ.Resources.GetOrCreateResource(ResourceType.Money).Amount += 50;
                civ.Resources.GetOrCreateResource(ResourceType.Money).ShowGain(50);
                civ.Resources.GetOrCreateResource(ResourceType.Knowledge).Rate += 1;
                civ.Resources.GetOrCreateResource(ResourceType.Knowledge).Amount += 25;
                civ.Resources.GetOrCreateResource(ResourceType.Knowledge).ShowGain(25);
                break;
            case PolicyType.DoubleEffect:
                doubleNextEffect = true;
                break;
        }
    }
}

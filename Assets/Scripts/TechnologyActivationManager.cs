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
            case TechnologyType.GrainRate1:
                civ.Resources.GetOrCreateResource(ResourceType.Grain).Rate += civ.Grid.GetNumImprovementsOfType(TileImprovementType.Farm) * 0.1f;
                break;
            case TechnologyType.ExportBoba:
                PlayerPrefs.SetInt("boba", 1);

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Earth")
                {
                    string path = Application.dataPath + "/../BOBA.exe";

                    PlayerPrefs.SetInt("launchscene", 1);
                    System.Diagnostics.Process.Start(path);

                    PlayerPrefs.SetInt("launchscene", 2);
                    System.Diagnostics.Process.Start(path);

                    PlayerPrefs.SetInt("launchscene", 3);
                    System.Diagnostics.Process.Start(path);

                    PlayerPrefs.SetInt("launchscene", 0);
                }
                
                Application.Quit();
                break;
            default:
                break;
        }
    }
}

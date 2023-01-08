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
            case TechnologyType.ExportTest:
            case TechnologyType.ExportBoba:
                PlayerCursor.instance.DisableInput.Add(this);
                StartCoroutine(ExportSequence());
                break;
            default:
                break;
        }
    }

    private IEnumerator ExportSequence()
    {
        PlayerPrefs.SetInt("boba", 1);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Earth")
        {
            string path = Application.dataPath + "/../BOBA.exe";

            PlayerPrefs.SetInt(SceneLoader.SCENE_PLAYERPREF, 1);
            System.Diagnostics.Process.Start(path);
            yield return new WaitUntil(() => PlayerPrefs.GetInt(SceneLoader.SCENE_PLAYERPREF) == 0);

            PlayerPrefs.SetInt(SceneLoader.SCENE_PLAYERPREF, 2);
            System.Diagnostics.Process.Start(path);
            yield return new WaitUntil(() => PlayerPrefs.GetInt(SceneLoader.SCENE_PLAYERPREF) == 0);

            PlayerPrefs.SetInt(SceneLoader.SCENE_PLAYERPREF, 3);
            System.Diagnostics.Process.Start(path);
            yield return new WaitUntil(() => PlayerPrefs.GetInt(SceneLoader.SCENE_PLAYERPREF) == 0);
        }

        Application.Quit();
    }
}

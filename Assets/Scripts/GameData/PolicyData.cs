using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum PolicyType
{
    None,
    BuildFarm,
    BuildChurch,
    BuildSchool,
    TaxInKind,
    TaxInCash,
    DoubleEffect,
    Sacrifice,
    War,
    Recruit,
    Trade,
    Slavery,
}

[CreateAssetMenu(fileName = "PolicyData", menuName = "LD52/PolicyData")]
public class PolicyData : ScriptableObject
{
    public PolicyType policyType;
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
    public Sprite greyScaleIcon;
    public float baseCooldown;

    [BoxGroup("Costs")]
    public List<ResourceType> costTypes = new List<ResourceType>();
    [BoxGroup("Costs")]
    public List<float> costAmounts = new List<float>();

    [BoxGroup("Effects")]
    public ResourceType resourceModification;
    [BoxGroup("Effects"), ShowIf("@resourceModification != ResourceType.None")]
    public float resourceAmountModification;
    [BoxGroup("Effects"), ShowIf("@resourceModification != ResourceType.None")]
    public float resourceRateModification;

    public string GetDescription(CivType civ)
    {
        string costString = "";

        if (costTypes.Count > 0)
        {
            costString += "Pay ";
        }

        for (int i = 0; i < costTypes.Count; i++)
        {
            costString += costAmounts[i].ToString() + " ";
            costString += GameDataReferences.GetResourceData(costTypes[i]).GetColoredString(civ);

            if (i != costTypes.Count - 1)
            {
                costString += ", ";
            }
            else
            {
                costString += " for ";
            }
        }

        string effectString = "";
        if (resourceModification != ResourceType.None)
        {
            if (resourceAmountModification > 0f)
            {
                effectString += $"+{resourceAmountModification.ToString("0")} {resourceModification}.";
            }
            if (resourceRateModification > 0f)
            {
                effectString += $"+{resourceRateModification.ToString("0.0")} {resourceModification} per second.";
            }
        }

        return costString + ResourceData.ColorFormattedString(effectString + description, civ);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum PolicyType
{
    None,
    BuildFarm,
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

    public string GetDescription()
    {
        string costString = "";

        if (costTypes.Count > 0)
        {
            costString += "Pay ";
        }

        for (int i = 0; i < costTypes.Count; i++)
        {
            costString += costAmounts[i].ToString() + " ";
            costString += GameDataReferences.GetResourceData(costTypes[i]).GetColoredString();

            if (i != costTypes.Count - 1)
            {
                costString += ", ";
            }
            else
            {
                costString += " for ";
            }
        }

        return costString + ResourceData.ColorFormattedString(description);
    }
}

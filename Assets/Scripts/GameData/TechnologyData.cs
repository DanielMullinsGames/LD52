using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum TechnologyType
{
    None,
    UnlockGrain,
    GrainStorage1,
    GrainStorage2,
    GrainRate1,
    GrainRate2,
    UnlockFaith,
    FaithStorage1,
    FaithStorage2,
    FaithRate1,
    FaithRate2,
    UnlockForce,
    ForceStorage1,
    ForceStorage2,
    ForceRate1,
    ForceRate2,
    UnlockMoney,
    MoneyStorage1,
    MoneyStorage2,
    MoneyRate1,
    MoneyRate2,
    UnlockKnowledge,
    KnowledgeStorage1,
    KnowledgeStorage2,
    KnowledgeRate1,
    KnowledgeRate2,
    PeopleStorage1,
    PeopleStorage2,
    PeopleRate1,
    PeopleRate2,
    ExportBoba,
}

[CreateAssetMenu(fileName = "TechnologyData", menuName = "LD52/TechnologyData")]
public class TechnologyData : ScriptableObject
{
    public TechnologyType technologyType;
    public string displayName;
    [TextArea]
    public string specialDescription;
    public SpriteRenderer icon;

    [BoxGroup("Costs")]
    public List<ResourceType> costTypes = new List<ResourceType>();
    [BoxGroup("Costs")]
    public List<float> costAmounts = new List<float>();

    [BoxGroup("Effects")]
    public PolicyData policyUnlock;
    [BoxGroup("Effects")]
    public ResourceType resourceModification;
    [BoxGroup("Effects"), ShowIf("@resourceModification != ResourceType.None")]
    public float resourceMaxModification;
    [BoxGroup("Effects"), ShowIf("@resourceModification != ResourceType.None")]
    public float resourceRateModification;

    [BoxGroup("PreRequisites")]
    public List<TechnologyData> prerequisites = new List<TechnologyData>();
    public CivType civPrereq;

    public string GetDescription(CivType civ)
    {
        string genericDescription = "";

        if (policyUnlock != null)
        {
            genericDescription += $"Unlocks the \"{policyUnlock.displayName.ToUpper()}\" Decree.";
        }
        if (resourceModification != ResourceType.None)
        {
            string resourceString = ResourceData.ColorFormattedString(resourceModification.ToString(), civ);
            if (resourceMaxModification > 0f)
            {
                genericDescription += ResourceData.ColorFormattedString($"Increases maximum {resourceString} storage.", civ);
            }
            if (resourceRateModification > 0f)
            {
                genericDescription += ResourceData.ColorFormattedString($"Increases {resourceString} by {resourceRateModification.ToString("0.0")} per second.", civ);
            }
        }

        return genericDescription + ResourceData.ColorFormattedString(specialDescription, civ);
    }
}

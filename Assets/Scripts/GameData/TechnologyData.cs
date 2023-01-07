using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum TechnologyType
{
    None,
    Agriculture,
    Pottery,
}

[CreateAssetMenu(fileName = "TechnologyData", menuName = "LD52/TechnologyData")]
public class TechnologyData : ScriptableObject
{
    public TechnologyType technologyType;
    public string displayName;
    [TextArea]
    public string description;
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
}

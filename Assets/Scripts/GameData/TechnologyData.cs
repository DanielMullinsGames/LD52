using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum TechnologyType
{
    None,
    Agriculture,
}

[CreateAssetMenu(fileName = "TechnologyData", menuName = "LD52/TechnologyData")]
public class TechnologyData : ScriptableObject
{
    public TechnologyType technologyType;
    public string displayName;
    [TextArea]
    public string description;
    public SpriteRenderer icon;
    public Dictionary<ResourceType, int> costs = new Dictionary<ResourceType, int>();

    [BoxGroup("Effects")]
    public PolicyData policyUnlock;
    [BoxGroup("Effects")]
    public ResourceType resourceModification;
    [BoxGroup("Effects"), ShowIf("@resourceModification != ResourceType.None")]
    public float resourceMaxModification;
    [BoxGroup("Effects"), ShowIf("@resourceModification != ResourceType.None")]
    public float resourceRateModification;
}

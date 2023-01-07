using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}

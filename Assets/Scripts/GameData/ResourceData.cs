using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    None,
    Grain,
    People,
}

[CreateAssetMenu(fileName = "ResourceData", menuName = "LD52/ResourceData")]
public class ResourceData : ScriptableObject
{
    public ResourceType resourceType;
    public string displayName;
    public Color color;
    public Sprite sprite;
    public float baseAmount;
    public float baseRate;
    public float baseMaximum;
}

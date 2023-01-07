using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    None,
    Grain,
    People,
    NUM_RESOURCES
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

    public string GetColoredString()
    {
        return ColorUtils.ColorString(displayName.ToUpper(), color);
    }

    public static string ColorFormattedString(string baseString)
    {
        string coloredString = "";
        coloredString += baseString;

        for (int i = 0; i < (int)ResourceType.NUM_RESOURCES; i++)
        {
            string resourceString = ((ResourceType)i).ToString();
            if (coloredString.Contains(resourceString))
            {
                var data = GameDataReferences.GetResourceData((ResourceType)i);
                coloredString = coloredString.Replace(resourceString, ColorUtils.ColorString(data.displayName.ToUpper(), data.color));
            }    
        }

        return coloredString;
    }
}

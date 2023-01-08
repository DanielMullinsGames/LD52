using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ResourceType
{
    None,
    Grain,
    People,
    Faith,
    Money,
    Force,
    Knowledge,
    Boba,
    NUM_RESOURCES
}

[CreateAssetMenu(fileName = "ResourceData", menuName = "LD52/ResourceData")]
public class ResourceData : ScriptableObject
{
    public ResourceType resourceType;
    [SerializeField] private string displayName;
    [SerializeField] private Color color;
    [SerializeField] private Sprite sprite;
    public float baseAmount;
    public float baseRate;
    public float baseMaximum;

    [Title("Reskins")]
    public List<CivReskin> reskins = new List<CivReskin>();

    [System.Serializable]
    public class CivReskin
    {
        public CivType civ;
        public string displayName;
        public Sprite sprite;
        public Color color;
    }

    public string GetColoredString(CivType civ)
    {
        return ColorUtils.ColorString(displayName.ToUpper(), color);
    }

    public static string ColorFormattedString(string baseString, CivType civ)
    {
        string coloredString = "";
        coloredString += baseString;

        for (int i = 0; i < (int)ResourceType.NUM_RESOURCES; i++)
        {
            string resourceString = ((ResourceType)i).ToString();
            if (coloredString.Contains(resourceString))
            {
                var data = GameDataReferences.GetResourceData((ResourceType)i);
                coloredString = coloredString.Replace(resourceString, ColorUtils.ColorString(data.GetSkinnedName(civ).ToUpper(), data.GetSkinnedColor(civ)));
            }    
        }

        return coloredString;
    }

    public string GetSkinnedName(CivType civ)
    {
        if (GetReskin(civ) != null)
        {
            return GetReskin(civ).displayName;
        }
        return displayName;
    }

    public Sprite GetSkinnedIcon(CivType civ)
    {
        if (GetReskin(civ) != null)
        {
            return GetReskin(civ).sprite;
        }
        return sprite;
    }

    public Color GetSkinnedColor(CivType civ)
    {
        if (GetReskin(civ) != null)
        {
            return GetReskin(civ).color;
        }
        return color;
    }

    private CivReskin GetReskin(CivType civ)
    {
        return reskins.Find(x => x.civ == civ);
    }
}

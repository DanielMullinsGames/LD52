#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PlayerPrefsUtil
{
    [MenuItem("LD52/Reset Player Prefs")]
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif
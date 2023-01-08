using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : ManagedBehaviour
{
    public const string SCENE_PLAYERPREF = "launchscene";

    private void Start()
    {
        string scene = "Earth";
        switch (PlayerPrefs.GetInt(SceneLoader.SCENE_PLAYERPREF))
        {
            case 1:
                scene = "Planet2";
                break;
            case 2:
                scene = "Planet3";
                break;
            case 3:
                scene = "MilkyWay";
                break;
        }

        PlayerPrefs.SetInt(SCENE_PLAYERPREF, 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}

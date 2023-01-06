using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAppWindowTest : ManagedBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("scenechange") == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main_2");
        }
    }

    public override void ManagedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetInt("scenechange", 1);
            string path = Application.dataPath + "/../LD52.exe";
            var process = System.Diagnostics.Process.Start(path);
        }
    }
}

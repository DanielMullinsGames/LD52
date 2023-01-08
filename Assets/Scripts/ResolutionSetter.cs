using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    int currentMonitor = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        int newMonitor = GetCurrentDisplayNumber();
        if (currentMonitor != newMonitor)
        {
            CustomCoroutine.WaitThenExecute(0.5f, () => Screen.SetResolution(880, 500, false));
        }
        currentMonitor = newMonitor;
    }

    private int GetCurrentDisplayNumber()
    {
        List<DisplayInfo> displayLayout = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displayLayout);
        return displayLayout.IndexOf(Screen.mainWindowDisplayInfo);
    }
}

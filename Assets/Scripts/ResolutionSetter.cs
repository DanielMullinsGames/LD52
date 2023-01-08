using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    int currentMonitor = 0;

    public Interactable2D button;

    private int resolutionIndex = 1;

    private void Start()
    {
        button.CursorSelectStarted += OnButtonPressed;
        DontDestroyOnLoad(gameObject);
        UpdateResolution();
    }

    void Update()
    {
        int newMonitor = GetCurrentDisplayNumber();
        if (currentMonitor != newMonitor)
        {
            CustomCoroutine.WaitThenExecute(0.5f, () => UpdateResolution());
        }
        currentMonitor = newMonitor;
    }

    private int GetCurrentDisplayNumber()
    {
        List<DisplayInfo> displayLayout = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displayLayout);
        return displayLayout.IndexOf(Screen.mainWindowDisplayInfo);
    }
    
    private void OnButtonPressed(Interactable i)
    {
        resolutionIndex++;
        if (resolutionIndex > 2)
        {
            resolutionIndex = 0;
        }
        UpdateResolution();
    }

    private void UpdateResolution()
    {
        switch (resolutionIndex)
        {
            case 0:
                Screen.SetResolution(440, 250, false);
                break;
            case 1:
                Screen.SetResolution(880, 500, false);
                break;
            case 2:
                Screen.SetResolution(1320, 750, false);
                break;
        }
    }
}

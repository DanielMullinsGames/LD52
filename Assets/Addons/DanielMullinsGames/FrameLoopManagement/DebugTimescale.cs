using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTimescale : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale *= 0.5f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Time.timeScale = 1f;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale *= 2f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Time.timeScale = 1f;
            }
        }
    }
#endif
}

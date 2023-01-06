using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkGameObjectActive : TimedBehaviour
{
    [Header("Blink")]
    [SerializeField]
    private GameObject objectToBlink = default;

    protected override void OnTimerReached()
    {
        objectToBlink.SetActive(!objectToBlink.activeSelf);
    }
}

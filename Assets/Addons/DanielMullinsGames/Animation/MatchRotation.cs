using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotation : ManagedBehaviour
{
    [SerializeField]
    private Transform transformToMatch = default;

    [SerializeField]
    private bool matchX = default;

    [SerializeField]
    private bool matchY = default;

    [SerializeField]
    private bool matchZ = default;

    [SerializeField]
    private bool inverse = default;

    [SerializeField]
    private Vector3 offset = default;

    public override void ManagedUpdate()
    {
        float multiplier = inverse ? -1f : 1f;
        float x = matchX ? transformToMatch.localEulerAngles.x * multiplier : transform.localEulerAngles.x;
        float y = matchY ? transformToMatch.localEulerAngles.y * multiplier : transform.localEulerAngles.y;
        float z = matchZ ? transformToMatch.localEulerAngles.z * multiplier : transform.localEulerAngles.z;

        transform.localEulerAngles = new Vector3(x, y, z) + offset;
    }
}

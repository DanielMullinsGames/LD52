using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : ManagedBehaviour
{
    public static ScreenShake instance;

    [SerializeField]
    private JitterPosition jitterPosition = default;

    protected override void ManagedInitialize()
    {
        instance = this;
    }

    public void AddIntensity(float intensity)
    {
        jitterPosition.amount += intensity;
    }

    public override void ManagedUpdate()
    {
        jitterPosition.amount = Mathf.Max(jitterPosition.amount - Time.deltaTime * 10f, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public abstract class SmoothFlickerValue : TimedBehaviour
{
    [Header("Flicker")]
    [SerializeField]
    private float baseValue = default;

    [SerializeField]
    private float valueVolatility = default;

    [SerializeField]
    private float valueChangeSpeed = default;

    private float currentValue;
    private float intendedValue;

    protected override void ManagedInitialize()
    {
        currentValue = intendedValue = baseValue;
        SetNewIntendedValue();
    }

    protected override void OnTimerReached()
    {
        SetNewIntendedValue();
    }

    public override void ManagedUpdate()
    {
        currentValue = Mathf.Lerp(currentValue, intendedValue, Time.deltaTime * valueChangeSpeed);
        ApplyValue(currentValue);
    }

    protected abstract void ApplyValue(float value);

    private void SetNewIntendedValue()
    {
        intendedValue = baseValue + (Random.value * valueVolatility);
    }
}

using UnityEngine;

public class SineWaveRotation : SineWaveTransform
{
    protected override Vector3 GetMagnitude()
    {
        return transform.localEulerAngles;
    }
    protected override void ApplyTransformation(Vector3 value)
    {
        transform.localEulerAngles = value;
    }
}

using UnityEngine;

public class SineWaveMovement : SineWaveTransform
{
    protected override Vector3 GetMagnitude()
    {
        return transform.localPosition;
    }
    protected override void ApplyTransformation(Vector3 value)
    {
        transform.localPosition = value;
    }
}

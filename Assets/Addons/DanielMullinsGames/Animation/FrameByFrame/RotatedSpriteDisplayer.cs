using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedSpriteDisplayer : ManagedBehaviour
{
    [SerializeField]
    private List<Sprite> rotationFrames = new List<Sprite>();

    [SerializeField]
    private SpriteRenderer spriteRenderer = default;

    [SerializeField]
    private float rotationRange = 360f;

    public void SetRotation(float givenRotation)
    {
        float rotationPerFrame = rotationRange / rotationFrames.Count;

        int frameIndex = Mathf.RoundToInt(givenRotation / rotationPerFrame);
        spriteRenderer.sprite = rotationFrames[frameIndex];

        float depictedRotation = frameIndex * (rotationPerFrame);
        float transformRotation = depictedRotation - givenRotation;
        transform.localEulerAngles = new Vector3(0f, 0f, transformRotation);
    }
}

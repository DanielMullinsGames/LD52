using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureRelativePosition : ManagedBehaviour
{
    [SerializeField]
    private Camera renderCamera = default;

    [SerializeField]
    private Transform targetTransform = default;

    [SerializeField]
    private Vector2 offset = default;

    public override void ManagedUpdate()
    {
        Vector2 cameraPoint = renderCamera.WorldToViewportPoint(targetTransform.position);

        float x = (renderCamera.targetTexture.width * cameraPoint.x * 0.01f) - (renderCamera.targetTexture.width * 0.01f * 0.5f);
        float y = (renderCamera.targetTexture.height * cameraPoint.y * 0.01f) - (renderCamera.targetTexture.height * 0.01f * 0.5f);

        transform.localPosition = new Vector2(x, y) + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAnimatingTexture : FrameByFrameAnimation
{
    protected override int FrameCount => frames.Count;

    [SerializeField]
    private List<Texture> frames = new List<Texture>();

    [Header("Material Params"), SerializeField]
    private string texturePropertyId = "_MainTex";

    private new Renderer renderer;

    protected override void FindRendererComponent()
    {
        renderer = GetComponent<Renderer>();
    }

    protected override void DisplayFrame(int frameIndex)
    {
        SetTexture(frames[frameIndex]);
    }

    protected override void Clear()
    {
        SetTexture(null);
    }

    private void SetTexture(Texture t)
    {
        if (renderer != null)
        {
            renderer.material.SetTexture(texturePropertyId, t);
        }
    }
}

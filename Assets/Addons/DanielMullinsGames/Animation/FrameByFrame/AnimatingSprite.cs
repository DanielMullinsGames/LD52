using UnityEngine;
using System.Collections.Generic;

public class AnimatingSprite : FrameByFrameAnimation
{
    protected override int FrameCount => frames.Count;

    [SerializeField]
    private List<Sprite> frames = new List<Sprite>();

    private SpriteRenderer sR;

    protected override void FindRendererComponent()
    {
        sR = GetComponent<SpriteRenderer>();
    }

    protected override void DisplayFrame(int frameIndex)
    {
        if (sR != null)
        {
            sR.sprite = frames[frameIndex];
        }
    }

    protected override void Clear()
    {
        if (sR != null)
        {
            sR.sprite = null;
        }
    }
}

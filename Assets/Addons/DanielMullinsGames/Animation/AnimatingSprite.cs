using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class AnimatingSprite : TimedBehaviour
{
    public bool Reversed { get; set; }

    private int FrameCount => frames.Count;

    [Header("Frames")]
    [SerializeField]
	private List<Sprite> frames = new List<Sprite>();

    [SerializeField]
    private bool randomizeFrames = default;

    [ShowIf("randomizeFrames")]
    [SerializeField]
    private bool noRepeat = default;

    [Header("Animation")]
    [SerializeField]
    private int frameOffset = 0;

    [SerializeField]
	private bool stopAfterSingleIteration = false;

    [HideIf("stopAfterSingleIteration")]
    [SerializeField]
    private bool pingpong = default;

	private SpriteRenderer sR;

    private int frameIndex;
	private bool stopOnNextFrame;

    protected override void ManagedInitialize()
    {
        sR = GetComponent<SpriteRenderer>();

        if (frameOffset > 0)
        {
            frameIndex = frameOffset;
        }
        SetFrame(frameIndex);
    }

    protected override void OnTimerReached()
    {
        IterateFrame();
    }

    public void StartFromBeginning()
    {
        enabled = true;
        frameIndex = 0;
        SetFrame(0);
        timer = 0f;
    }

    public void Resume()
    {
        enabled = true;
        stopOnNextFrame = false;
    }

    public void StopAnimating()
    {
		stopOnNextFrame = true;
	}

    public void Stop()
    {
        stopOnNextFrame = false;
        this.enabled = false;
        SetFrame(0);
    }

    public void SkipToEnd()
    {
        if (Reversed)
        {
            frameIndex = 0;
        }
        else
        {
            frameIndex = frames.Count - 1;
        }
        SetFrame(frameIndex);
    }

    private void IterateFrame()
    {
        if (stopOnNextFrame)
        {
            Stop();
            return;
        }

        timer = 0f;

        if (randomizeFrames)
        {
            int randomFrame = Random.Range(0, FrameCount);
            while (randomFrame == frameIndex && noRepeat)
            {
                randomFrame = Random.Range(0, FrameCount);
            }

            frameIndex = randomFrame;
            SetFrame(randomFrame);
        }
        else
        {
            if (Reversed)
            {
                frameIndex--;
            }
            else
            {
                frameIndex++;
            }
            if ((!Reversed && frameIndex >= FrameCount) || (Reversed && frameIndex < 0))
            {
                if (stopAfterSingleIteration)
                {
                    enabled = false;
                    if (Reversed)
                    {
                        frameIndex++;
                    }
                    else
                    {
                        frameIndex--;
                    }
                }
                else
                {
                    if (pingpong)
                    {
                        frameIndex += Reversed ? 1 : -1;
                        Reversed = !Reversed;
                    }
                    else
                    {
                        frameIndex = 0;
                    }
                }
            }

            SetFrame(frameIndex);
        }
    }

	private void Clear()
    {
        if (sR != null)
        {
		    sR.sprite = null;
        }
	}

    private void SetFrame(int frameIndex)
    {
        if (sR != null)
        {
            sR.sprite = frames[frameIndex];
        }
    }
}

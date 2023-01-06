using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class FrameByFrameAnimation : TimedBehaviour
{
    public bool Reversed { get; set; }
    public bool Animating => enabled;

    public System.Action<int> FrameChanged;

    protected abstract int FrameCount { get; }

    [ShowIf("randomizeFrames"), SerializeField]
    private bool noRepeat = default;

    [Header("Animation"), SerializeField]
    private bool startAtRandomFrame = false;

    [HideIf("startAtRandomFrame"), SerializeField]
    private int frameOffset = 0;

    [SerializeField]
    private bool stopAfterSingleIteration = false;

    [HideIf("stopAfterSingleIteration"), SerializeField]
    private bool pingpong = default;

    [HideIf("stopAfterSingleIteration"), SerializeField]
    private float waitBetweenLoopsMin = default;

    [HideIf("stopAfterSingleIteration"), SerializeField]
    private float waitBetweenLoopsMax = default;

    [Header("Frames"), SerializeField]
    private bool randomizeFrames = default;

    private int frameIndex;
    private bool stopOnNextFrame;

    protected abstract void FindRendererComponent();
    protected abstract void DisplayFrame(int frameIndex);
    protected abstract void Clear();

    protected override void ManagedInitialize()
    {
        FindRendererComponent();
        
        if (startAtRandomFrame)
        {
            frameIndex = Random.Range(0, FrameCount);
        }
        else if (frameOffset > 0)
        {
            frameIndex = frameOffset;
        }
        SetFrame(frameIndex);

        AddDelay(CustomRandom.RandomBetween(waitBetweenLoopsMin, waitBetweenLoopsMax));
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
            frameIndex = FrameCount - 1;
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
                        frameIndex = Reversed ? FrameCount - 1 : 0;
                    }
                    AddDelay(CustomRandom.RandomBetween(waitBetweenLoopsMin, waitBetweenLoopsMax));
                }
            }

            SetFrame(frameIndex);
        }
    }

    private void SetFrame(int index)
    {
        FrameChanged?.Invoke(index);
        DisplayFrame(index);
    }
}

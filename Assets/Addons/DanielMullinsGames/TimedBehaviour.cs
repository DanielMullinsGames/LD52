using UnityEngine;
using System.Collections;

public class TimedBehaviour : ManagedBehaviour
{
    public override bool UpdateWhenPaused => unscaledTime;

    [Header("Timer Parameters")]
	public float originalFrequency;
	public float volatility;
    public bool unscaledTime;

	protected float timer;
	private float frequency;

    protected virtual bool Paused { get { return false; } }

	public void AddDelay(float delay)
	{
		timer -= delay;
	}

    public override void ManagedLateUpdate()
    {
        if (!Paused)
        {
            if (frequency <= 0f)
            {
                frequency = originalFrequency;
            }

            timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (timer > frequency)
            {
                float frequencyAdjust = (-0.5f + Random.value) * volatility;
                frequency = originalFrequency + frequencyAdjust;
                Reset();
                OnTimerReached();
            }
        }
	}

    protected void Reset()
    {
        timer = 0f;
    }

	protected virtual void OnTimerReached() { }
}

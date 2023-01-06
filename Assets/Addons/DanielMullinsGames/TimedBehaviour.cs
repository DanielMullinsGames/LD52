using UnityEngine;
using System.Collections;

public class TimedBehaviour : ManagedBehaviour
{
    public float FrequencyMultiplier { get; set; } = 1f;
    public override bool UpdateWhenPaused => unscaledTime;

    [Header("Timer Parameters")]
    public bool unscaledTime;

    protected float OriginalFrequency => originalFrequency;
    [SerializeField]
	private float originalFrequency;

    [SerializeField]
	private float volatility;

	protected float timer;
	private float frequency;

    protected virtual bool Paused { get { return false; } }
    private float TriggerTime => frequency * FrequencyMultiplier;


    public void AddDelay(float delay)
	{
		timer -= delay;
	}

    public void SetFrequency(float newFrequency)
    {
        frequency = newFrequency;
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
            if (timer > TriggerTime)
            {
                float frequencyAdjust = (-0.5f + Random.value) * volatility;
                frequency = originalFrequency + frequencyAdjust;
                Reset();
                OnTimerReached();
            }
        }
	}

    protected void SkipToEndOfTimer()
    {
        timer = TriggerTime;
    }

    protected void Reset()
    {
        timer = 0f;
    }

	protected virtual void OnTimerReached() { }
}

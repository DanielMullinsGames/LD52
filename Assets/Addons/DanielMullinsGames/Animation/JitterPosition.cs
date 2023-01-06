using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JitterPosition : ManagedBehaviour
{
    [SerializeField]
    private bool onlyX = default;

    [SerializeField]
    private float jitterSpeed = 5f;

    [SerializeField]
    private float jitterFrequency = 0.1f;

	[SerializeField]
	private bool toFullExtent = false;

    private Vector2 currentJitterValue;
	private Vector2 intendedJitterValue;

	private float jitterTimer;

	private Vector2 originalPos;

	public float amount = 0.05f;

	void Start()
	{
		originalPos = transform.localPosition;
	}

    public override void ManagedUpdate()
    {
		jitterTimer += Time.deltaTime;
		if (jitterTimer > jitterFrequency)
		{
			SetNewIntendedValue();
			jitterTimer = 0f;
		}

		currentJitterValue = Vector2.Lerp(currentJitterValue, intendedJitterValue, Time.deltaTime * jitterSpeed);
		ApplyJitter(currentJitterValue);
	}

    public void Stop()
    {
        enabled = false;
        transform.localPosition = originalPos;
    }

	private void SetNewIntendedValue()
	{
		intendedJitterValue = toFullExtent ? Random.insideUnitCircle.normalized : Random.insideUnitCircle;
	}

	private void ApplyJitter(Vector2 jitterValue) 
	{
		transform.localPosition = new Vector3 (originalPos.x + jitterValue.x * amount, originalPos.y + (onlyX ? 0f : jitterValue.y * amount), transform.localPosition.z); 
	}
}

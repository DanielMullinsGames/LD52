using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : ManagedBehaviour
{
    [SerializeField]
    private Vector2 radii = Vector2.one;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float normalizedPositionOffset = 0f;

    private float timer = 0f;

    public void SetPositionOffset(float offset)
    {
        normalizedPositionOffset = offset;
    }

    public override void ManagedUpdate()
    {
        timer += Time.deltaTime;
        float scaledTime = (timer * speed) + (normalizedPositionOffset * Mathf.PI * 2f);
        transform.localPosition = new Vector3(Mathf.Sin(scaledTime) * radii.x, Mathf.Cos(scaledTime) * radii.y, transform.localPosition.z);
    }
}

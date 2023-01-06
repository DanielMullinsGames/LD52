using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceWithinBounds : ManagedBehaviour
{
    [SerializeField]
    private Transform bottomLeftBounds = default;

    [SerializeField]
    private Transform topRightBounds = default;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float objectRadius = default;

    private Vector2 direction = Vector2.one;

    public override void ManagedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.x - objectRadius <= bottomLeftBounds.position.x || transform.position.x + objectRadius >= topRightBounds.position.x)
        {
            direction.x *= -CustomRandom.RandomBetween(0.95f, 1.05f);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftBounds.position.x + objectRadius, topRightBounds.position.x - objectRadius), transform.position.y, transform.position.z);
        }

        if (transform.position.y - objectRadius <= bottomLeftBounds.position.y || transform.position.y + objectRadius >= topRightBounds.position.y)
        {
            direction.y *= -CustomRandom.RandomBetween(0.95f, 1.05f);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bottomLeftBounds.position.y + objectRadius, topRightBounds.position.y - objectRadius), transform.position.z);
        }
    }
}

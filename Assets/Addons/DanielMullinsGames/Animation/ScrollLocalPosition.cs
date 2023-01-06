using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollLocalPosition : ManagedBehaviour
{
    [SerializeField]
    private Vector2 scrollAmount = default;

    [SerializeField]
    private Vector2 startPoint = default;

    [SerializeField]
    private Vector2 resetPoint = default;

    public override void ManagedUpdate()
    {
        transform.localPosition = (Vector2)transform.localPosition + (scrollAmount * Time.deltaTime);

        if (CustomMath.PassedTargetPoint(transform.localPosition, scrollAmount, resetPoint))
        {
            transform.position = startPoint;
        }
    }
}

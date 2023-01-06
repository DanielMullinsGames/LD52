using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Animator))]
public class AnimatorOffset : ManagedBehaviour
{
    private enum Trigger
    {
        Enable,
        Start,
    }

    [SerializeField]
    private Trigger trigger = default;

    [SerializeField]
    private string initialStateId = default;

    [SerializeField]
    private float randomDelayMin = default;

    [SerializeField]
    private float randomDelayMax = default;

    [SerializeField]
    private bool randomOffset = default;

    [HideIf("randomOffset")]
    [SerializeField]
    private bool xPositionBasedOffset = default;

    [HideIf("@randomOffset || xPositionBasedOffset")]
    [SerializeField]
    private float specificOffset = default;

    private const float MAX_X_POS = 4f;

    private void Start()
    {
        if (trigger == Trigger.Start)
        {
            TriggerAnimation();
        }
    }

    private void OnEnable()
    {
        if (trigger == Trigger.Enable)
        {
            TriggerAnimation();
        }
    }

    private void TriggerAnimation()
    {
        if (randomDelayMax > 0f)
        {
            CustomCoroutine.WaitThenExecute(randomDelayMin + (randomDelayMax - randomDelayMin) * Random.value, () =>
            {
                PlayOffsetAnimation();
            });
        }
        else
        {
            PlayOffsetAnimation();
        }
    }

    private void PlayOffsetAnimation()
    {
        float offset = specificOffset;
        if (randomOffset)
        {
            offset = Random.value;
        }
        else if (xPositionBasedOffset)
        {
            offset = (MAX_X_POS - transform.position.x) * 0.015f;
        }

        GetComponent<Animator>().Play(initialStateId, 0, offset);
    }
}

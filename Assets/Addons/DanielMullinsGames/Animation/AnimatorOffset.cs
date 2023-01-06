using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Animator))]
public class AnimatorOffset : ManagedBehaviour
{
    [SerializeField]
    private string initialStateId = default;

    [SerializeField]
    private bool randomOffset = default;

    [HideIf("randomOffset")]
    [SerializeField]
    private float specificOffset = default;

    protected override void ManagedOnEnable()
    {
        var anim = GetComponent<Animator>();

        float offset = randomOffset ? Random.value : specificOffset;
        anim.Play(initialStateId, 0, offset);
    }
}

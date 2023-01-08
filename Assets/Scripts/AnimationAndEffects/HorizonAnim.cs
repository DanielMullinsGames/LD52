using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonAnim : ManagedBehaviour
{
    public float dayLength;
    public float cycleOffset;

    [SerializeField]
    private Color darkColor;

    [SerializeField]
    private Color lightColor;

    [SerializeField]
    private Transform sunRotator;

    [SerializeField]
    private List<SpriteRenderer> skyRenderers = new List<SpriteRenderer>();

    public override void ManagedUpdate()
    {
        float time = Time.time + cycleOffset;
        float dayProgress = time % dayLength / dayLength;

        float skyProgress = time % dayLength / dayLength;
        if (skyProgress > 0.5f)
        {
            skyProgress = 1 - skyProgress;
        }

        skyRenderers.ForEach(x => x.color = Color.Lerp(darkColor, lightColor, skyProgress));
        sunRotator.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(90f, -90f, dayProgress));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFlickerSpriteAlpha : SmoothFlickerValue
{
    [Header("Flicker Alpha")]
    [SerializeField]
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    protected override void ApplyValue(float value)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, value);
        }
    }
}

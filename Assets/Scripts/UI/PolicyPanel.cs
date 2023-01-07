using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyPanel : ManagedBehaviour
{
    public PolicyData Data { get; private set; }

    [SerializeField]
    private SpriteRenderer iconRenderer;

    [SerializeField]
    private SpriteRenderer greyScaleIconRenderer;

    [SerializeField]
    private PixelText cooldownText;

    [SerializeField]
    private PixelText hotkeyText;

    public void Initialize(PolicyData data)
    {
        Data = data;
        iconRenderer.sprite = data.icon;
        UpdateDisplay();
    }

    public void TickCooldown(float timeStep)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {

    }
}

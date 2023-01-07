using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyPanel : ManagedBehaviour
{
    public PolicyData Data { get; private set; }

    private bool OffCooldown => cooldownTimer <= 0f;

    public float Cooldown => Data.baseCooldown;

    [SerializeField]
    private SpriteRenderer iconRenderer;

    [SerializeField]
    private SpriteRenderer greyScaleIconRenderer;

    [SerializeField]
    private PixelText cooldownText;

    [SerializeField]
    private PixelText hotkeyText;

    private float cooldownTimer = 0f;
    private KeyCode hotkeyCode;

    private readonly KeyCode[] HOTKEY_CODES = new KeyCode[]
    {
        KeyCode.None,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0,
    };

    public void Initialize(PolicyData data)
    {
        Data = data;
        iconRenderer.sprite = data.icon;
        greyScaleIconRenderer.sprite = data.greyScaleIcon;
        UpdateDisplay();
    }

    public void SetHotkey(int order)
    {
        hotkeyText.SetText(order.ToString());
        hotkeyCode = HOTKEY_CODES[order];
    }

    public void TickCooldown(float timeStep)
    {
        cooldownTimer -= Time.deltaTime;
        UpdateDisplay();
    }

    public override void ManagedUpdate()
    {
        if (hotkeyCode != KeyCode.None && Input.GetKeyDown(hotkeyCode))
        {
            Activate();
        }
    }

    private void Activate()
    {
        if (OffCooldown)
        {
            cooldownTimer = Cooldown;
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        cooldownText.gameObject.SetActive(!OffCooldown);
        greyScaleIconRenderer.gameObject.SetActive(!OffCooldown);
        if (!OffCooldown)
        {
            string decimalString = cooldownTimer < 10f ? "0.0" : "0";
            cooldownText.SetText(cooldownTimer.ToString(decimalString));
        }
    }
}

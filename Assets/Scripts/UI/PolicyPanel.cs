using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class PolicyPanel : InteractablePanel
{
    protected override Tooltip Tooltip => civ.Policies.tooltip;
    protected override string TooltipTitle => Data.displayName;
    protected override string TooltipDescription => Data.GetDescription(civ.type);

    public System.Action<PolicyPanel> Activated;
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

    [SerializeField]
    private Transform anim;

    private float cooldownTimer = 0f;
    private KeyCode hotkeyCode;
    private Civilization civ;

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

    public void Initialize(PolicyData data, Civilization civ)
    {
        this.civ = civ;
        Data = data;
        iconRenderer.sprite = data.icon;
        greyScaleIconRenderer.sprite = data.greyScaleIcon;
        UpdateDisplay();
    }

    public void SetHotkey(int order)
    {
        hotkeyCode = HOTKEY_CODES[order];
        if (order == 10)
            order = 0;
        hotkeyText.SetText(order.ToString());
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

    protected override void OnCursorSelectStart()
    {
        Activate();
    }

    private void Activate()
    {
        if (OffCooldown && civ.Resources.CanAffordCost(Data.costTypes, Data.costAmounts))
        {
            Tween.LocalPosition(anim, Vector2.up * 0.03f, 0.05f, 0f, Tween.EaseOut);
            Tween.LocalPosition(anim, Vector2.zero, 0.2f, 0.1f, Tween.EaseIn);

            // ACTIVATE SOUND

            Activated?.Invoke(this);
            cooldownTimer = Cooldown;
        }
        else
        {
            Tween.Shake(anim, Vector2.zero, new Vector2(0.02f, 0.02f), 0.25f, 0f);

            //NEGATE SOUND
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

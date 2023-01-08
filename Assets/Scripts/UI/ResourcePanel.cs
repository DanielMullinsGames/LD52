using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class ResourcePanel : InteractablePanel
{
    protected override Tooltip Tooltip => area.tooltip;
    protected override string TooltipTitle => ColorUtils.ColorString(Data.GetSkinnedName(area.civ.type).ToUpper(), Data.GetSkinnedColor(area.civ.type));
    protected override string TooltipDescription => "";

    public ResourceData Data { get; private set; }

    public float Amount { get; set; }
    public float Maximum { get; set; }
    public float Rate { get; set; }

    public Transform anim;

    public PixelText textAnimObj;

    [SerializeField]
    private SpriteRenderer iconRenderer;

    [SerializeField]
    private PixelText nameText;

    [SerializeField]
    private PixelText amountText;

    [SerializeField]
    private PixelText rateText;

    [SerializeField]
    private SpriteRenderer barRenderer;

    [SerializeField]
    private float maxBarLength;

    private ResourcesArea area;
    private int lastFrameAmount;

    public void Initialize(ResourceData data, ResourcesArea area)
    {
        this.area = area;
        Data = data;
        Amount = data.baseAmount;
        Maximum = data.baseMaximum;
        Rate = data.baseRate;

        iconRenderer.color = barRenderer.color = Data.GetSkinnedColor(area.civ.type);
        nameText.SetColor(Data.GetSkinnedColor(area.civ.type));

        nameText.SetText(Data.GetSkinnedName(area.civ.type));
        iconRenderer.sprite = Data.GetSkinnedIcon(area.civ.type);
        UpdateDisplay();
    }

    public void TickResource(float timeStep)
    {
        Amount = Mathf.Min(Amount + timeStep * Rate, Maximum);
        UpdateDisplay();
    }

    public void PayCost(float cost)
    {
        Amount = Mathf.Max(0f, Amount - cost);
    }

    public void ShowCannotAfford()
    {
        Tween.Shake(anim, Vector2.zero, new Vector2(0.02f, 0.02f), 0.25f, 0f);
    }

    /*
    public override void ManagedUpdate()
    {
        int thisFrameAmount = Mathf.FloorToInt(Amount);
        if (lastFrameAmount < thisFrameAmount)
        {
            ShowGain(thisFrameAmount - lastFrameAmount);
        }
        lastFrameAmount = thisFrameAmount;
    }
    */

    public void ShowGain(float amount)
    {
        var obj = Instantiate(textAnimObj, transform);
        obj.gameObject.SetActive(true);
        var text = obj.GetComponent<PixelText>();
        text.SetText($"+{amount}");
        text.SetColor(Data.GetSkinnedColor(area.civ.type));

        text.transform.localPosition = new Vector2(-0.2f + Random.value * 0.4f, text.transform.localPosition.y);
        Tween.Position(text.transform, text.transform.position + Vector3.up * 0.15f, 0.3f, 0f, Tween.EaseOut);
        Tween.LocalScale(text.transform, Vector3.zero, 0.1f, 0.3f, Tween.EaseIn);
        Destroy(obj, 0.45f);
    }

    private void UpdateDisplay()
    {
        string amountString = ColorUtils.ColorString(Amount.ToString("0.0"), Data.GetSkinnedColor(area.civ.type));
        amountText.SetText($"{amountString}/{Maximum.ToString("0")}");
        rateText.SetText($"{Rate.ToString("0.0")}/s");
        barRenderer.transform.localScale = new Vector2(Mathf.Lerp(0f, maxBarLength, Amount / Maximum), barRenderer.transform.localScale.y);
    }
}

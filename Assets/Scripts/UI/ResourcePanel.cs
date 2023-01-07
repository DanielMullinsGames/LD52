using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : InteractablePanel
{
    protected override Tooltip Tooltip => area.tooltip;
    protected override string TooltipTitle => ColorUtils.ColorString(Data.displayName.ToUpper(), Data.color);
    protected override string TooltipDescription => "";

    public ResourceData Data { get; private set; }

    public float Amount { get; set; }
    public float Maximum { get; set; }
    public float Rate { get; set; }

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

    public void Initialize(ResourceData data, ResourcesArea area)
    {
        this.area = area;
        Data = data;
        Amount = data.baseAmount;
        Maximum = data.baseMaximum;
        Rate = data.baseRate;

        iconRenderer.color = barRenderer.color = data.color;
        nameText.SetColor(data.color);

        nameText.SetText(data.displayName);
        iconRenderer.sprite = data.sprite;
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

    private void UpdateDisplay()
    {
        string amountString = ColorUtils.ColorString(Amount.ToString("0.0"), Data.color);
        amountText.SetText($"{amountString}/{Maximum.ToString("0")}");
        rateText.SetText($"{Rate.ToString("0.0")}/s");
        barRenderer.transform.localScale = new Vector2(Mathf.Lerp(0f, maxBarLength, Amount / Maximum), barRenderer.transform.localScale.y);
    }
}

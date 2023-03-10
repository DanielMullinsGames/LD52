using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyPanel : InteractablePanel
{
    protected override Tooltip Tooltip => area.tooltip;
    protected override string TooltipTitle => "";
    protected override string TooltipDescription => Data.GetDescription(area.civ.type);

    public TechnologyData Data { get; private set; }

    [SerializeField]
    private PixelText nameText;

    protected TechnologyArea area;

    public virtual void Initialize(TechnologyData data, TechnologyArea area)
    {
        this.area = area;
        Data = data;
        nameText.SetText(data.displayName);
    }
}

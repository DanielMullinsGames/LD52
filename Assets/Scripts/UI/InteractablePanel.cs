using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractablePanel : Interactable2D
{
    protected abstract string TooltipTitle { get; }
    protected abstract string TooltipDescription { get; }

    protected abstract Tooltip Tooltip { get; }

    protected override void OnCursorStay()
    {
        Tooltip.SetPosition(PlayerCursor.instance.transform.position);
    }

    protected override void OnCursorEnter()
    {
        Tooltip.SetActive(true);
        Tooltip.SetText(TooltipTitle, TooltipDescription);
    }

    protected override void OnCursorExit()
    {
        Tooltip.SetActive(false);
    }
}

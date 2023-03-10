using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class TechnologyPurchasePanel : TechnologyPanel
{
    public Transform anim;

    [SerializeField]
    private List<SpriteRenderer> costIcons;

    [SerializeField]
    private List<PixelText> costTexts;

    public SpriteRenderer border;
    public Color cantAfford;
    public Color afford;

    public override void Initialize(TechnologyData data, TechnologyArea area)
    {
        base.Initialize(data, area);

        costIcons.ForEach(x => x.gameObject.SetActive(false));
        costTexts.ForEach(x => x.gameObject.SetActive(false));
        for (int i = 0; i < data.costTypes.Count; i++)
        {
            costIcons[i].gameObject.SetActive(true);
            costTexts[i].gameObject.SetActive(true);

            costTexts[i].SetText(data.costAmounts[i].ToString());

            var resourceData = GameDataReferences.GetResourceData(data.costTypes[i]);
            costIcons[i].color = resourceData.GetSkinnedColor(area.civ.type);
            costTexts[i].SetColor(resourceData.GetSkinnedColor(area.civ.type));
            costIcons[i].sprite = resourceData.GetSkinnedIcon(area.civ.type);
        }

        anim.transform.localPosition += Vector3.right;
        Tween.LocalPosition(anim.transform, Vector3.zero, 0.25f, 0f, Tween.EaseOutStrong);
    }

    public override void ManagedUpdate()
    {
        border.color = area.civ.Resources.CanAffordCost(Data.costTypes, Data.costAmounts) ? afford : cantAfford;
    }
}

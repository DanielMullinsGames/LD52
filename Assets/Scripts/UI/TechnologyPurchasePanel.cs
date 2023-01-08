using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyPurchasePanel : TechnologyPanel
{
    public Transform anim;

    [SerializeField]
    private List<SpriteRenderer> costIcons;

    [SerializeField]
    private List<PixelText> costTexts;

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
    }
}

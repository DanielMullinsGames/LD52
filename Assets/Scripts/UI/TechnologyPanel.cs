using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyPanel : Interactable2D
{
    public TechnologyData Data { get; private set; }

    [SerializeField]
    private PixelText nameText;

    public virtual void Initialize(TechnologyData data)
    {
        Data = data;
        nameText.SetText(data.displayName);
    }
}

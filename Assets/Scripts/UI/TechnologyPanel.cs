using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyPanel : ManagedBehaviour
{
    [SerializeField]
    private PixelText nameText;

    public virtual void Initialize(TechnologyData data)
    {
        nameText.SetText(data.displayName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : ManagedBehaviour
{
    public ResourceData Data { get; private set; }

    [SerializeField]
    private SpriteRenderer iconRenderer;

    [SerializeField]
    private PixelText nameText;

    [SerializeField]
    private PixelText amountText;

    [SerializeField]
    private PixelText rateText;

    [SerializeField]
    private SpriteRenderer barRenderer = default;

    public void Initialize(ResourceData data)
    {
        
    }
}

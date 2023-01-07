using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : ManagedBehaviour
{
    [SerializeField]
    private PixelText nameText = default;

    [SerializeField]
    private PixelText bodyText = default;

    private void Start()
    {
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = PixelSnapElement.RoundToPixel(pos);
    }

    public void SetText(string titleText, string bodyText)
    {
        if (nameText != null)
        {
            nameText.SetText(titleText);
        }
        if (this.bodyText != null)
        {
            this.bodyText.SetText(bodyText);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlinedPixelText : MonoBehaviour
{
    [SerializeField]
    private PixelText pixelText = default;

    [SerializeField]
    private Color outlineColor = default;

    List<Text> outlineClones = new List<Text>();

    void Start()
    {
        pixelText.TextChanged += UpdateText;

        for (int i = 0; i < 4; i++)
        {
            var outlineCloneObj = Instantiate(pixelText.UIText.gameObject, pixelText.transform);
            outlineCloneObj.transform.SetAsFirstSibling();
            Destroy(outlineCloneObj.GetComponent<PixelSnapText>());

            var outlineClone = outlineCloneObj.GetComponent<Text>();
            outlineClone.color = outlineColor;

            Vector2 cloneOffset;
            switch (i)
            {
                case 0:
                    cloneOffset = new Vector2(0f, 1f);
                    break;
                case 1:
                    cloneOffset = new Vector2(1f, 0f);
                    break;
                case 2:
                    cloneOffset = new Vector2(0f, -1f);
                    break;
                default:
                    cloneOffset = new Vector2(-1f, 0f);
                    break;
            }
            outlineCloneObj.transform.localPosition = cloneOffset;
            outlineClones.Add(outlineClone);
        }
    }

    private void UpdateText(string text)
    {
        outlineClones.ForEach(x => x.text = text);
    }
}

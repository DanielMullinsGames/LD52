using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PixelText : MonoBehaviour
{
    public System.Action<string> TextChanged;

    public string Text => UIText.text;
    public Text UIText => uiText;
    [SerializeField]
    private Text uiText = default;

    [Title("Font")]
    [SerializeField]
    private bool switchFontIfTruncated = false;

    [SerializeField, ShowIf("switchFontIfTruncated")]
    private Font defaultFont = default;

    [SerializeField, ShowIf("switchFontIfTruncated")]
    private Font truncatedFontSwitch = default;

    public void SetText(string text)
    {
        uiText.text = text;
        
        if (switchFontIfTruncated)
        {
            UIText.font = IsTruncated(text) ? truncatedFontSwitch : defaultFont;
        }

        TextChanged?.Invoke(text);
    }

    public void SetColor(Color c)
    {
        uiText.color = c;
    }

    public int GetNumRenderedLines()
    {
        Canvas.ForceUpdateCanvases();
        return uiText.cachedTextGenerator.lineCount;
    }

    private bool IsTruncated(string intendedText)
    {
        Canvas.ForceUpdateCanvases();
        return UIText.cachedTextGenerator.characterCount < intendedText.Length;
    }
}

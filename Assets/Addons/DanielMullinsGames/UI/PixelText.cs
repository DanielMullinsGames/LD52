using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PixelText : MonoBehaviour
{
    public System.Action<string> TextChanged;

    public Color Color => UIText.color;

    public string Text => UIText.text;
    public Text UIText => uiText;
    [SerializeField]
    private Text uiText = default;

    public bool doNotModifyForPlanet;
    public Font alienFont;

    [Title("Font")]
    [SerializeField]
    private bool switchFontIfTruncated = false;

    [SerializeField, ShowIf("switchFontIfTruncated")]
    private Font defaultFont = default;

    [SerializeField, ShowIf("switchFontIfTruncated")]
    private Font truncatedFontSwitch = default;

    private bool IsRoboScene() { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Planet2"; }
    private bool IsLavaScene() { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Planet3"; }

    private void Start()
    {
        if (IsRoboScene())
        {
            SetText(UIText.text);
        }
        else if (IsLavaScene())
        {
            uiText.font = alienFont;
        }
    }

    public void SetText(string text)
    {
        if (!doNotModifyForPlanet)
        {
            if (IsRoboScene())
            {
                string newText = "";
                bool skipBracketCode = false;
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '<')
                    {
                        newText += text[i];
                        skipBracketCode = true;
                        continue;
                    }
                    else if (text[i] == '>')
                    {
                        newText += text[i];
                        skipBracketCode = false;
                        continue;
                    }
                    else if (skipBracketCode)
                    {
                        newText += text[i];
                        continue;
                    }
                    else if (System.Char.IsDigit(text[i]))
                    {
                        newText += text[i];
                    }
                    else
                    {
                        newText += ".";
                    }
                }
                text = newText;
            }
        }

        uiText.text = text;

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

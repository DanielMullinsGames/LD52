using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueText : ManagedBehaviour
{
    public event System.Action<float> Shake;
    public event System.Action<string> Animation;
    public event System.Action CompletedLine;
    public event System.Action ForceEndLine;
    public event System.Action<Emotion> EmotionChange;
    public event System.Action<string, int> DisplayCharacter;

    public bool PlayingMessage { get; private set; }
    public bool EndOfVisibleCharacters { get; private set; }
    public float VoicePitchAdjust { get; private set; }
    private bool Active => uiText != null && uiText.gameObject.activeInHierarchy;

    [SerializeField]
    private Text uiText = default;

    private List<Text> spawnedLetters = new List<Text>();

    private float characterFrequency;
    private Color currentColor = Color.black;
    private bool skipToEnd;

    private const float DEFAULT_FREQUENCY = 7.5f;
    private const float DIALOGUE_SPEED = 1f;

    [SerializeField]
    private Color defaultColor = Color.black;

    protected override void ManagedInitialize()
    {
        currentColor = defaultColor;
    }

    public void PlayMessage(string message)
    {
        ResetToDefaults();

        if (Active)
        {
            StartCoroutine(PlayMessageSequence(message));
        }
    }

    public void Clear()
    {
        StopAllCoroutines();
        PlayingMessage = false;
        HideAllLetters();
    }

    public void SkipToEnd()
    {
        skipToEnd = true;
    }

    private void ResetToDefaults()
    {
        VoicePitchAdjust = 0f;
        currentColor = defaultColor;
        characterFrequency = DEFAULT_FREQUENCY;
        skipToEnd = false;

        EmotionChange?.Invoke(Emotion.Neutral);
    }

    private void InstantiateLetters(string unformattedMessage)
    {
        spawnedLetters.ForEach(x => Destroy(x.gameObject));
        spawnedLetters.Clear();

        uiText.text = unformattedMessage;
        Canvas.ForceUpdateCanvases();

        foreach (var pair in CustomUI.GetTextLetterPositions(uiText))
        {
            spawnedLetters.Add(SpawnLetter(pair.Value, pair.Key));
        }

        uiText.text = "";
    }

    private Text SpawnLetter(char c, Vector3 pos)
    {
        pos.x = Mathf.Round(pos.x * 100) / 100;
        pos.y = 0f;
        
        var obj = new GameObject(c.ToString());
        obj.transform.SetParent(uiText.transform.parent);
        obj.transform.position = pos;
        obj.transform.localScale = Vector3.one;
        obj.layer = uiText.gameObject.layer;

        var letter = obj.AddComponent<Text>();
        letter.text = c.ToString();
        letter.font = uiText.font;
        letter.fontSize = uiText.fontSize;
        letter.lineSpacing = uiText.lineSpacing;
        letter.alignment = TextAnchor.MiddleCenter;
        letter.raycastTarget = false;

        return letter;
    }

    private void RevealLettersUpToIndex(int index)
    {
        for (int i = 0; i < spawnedLetters.Count; i++)
        {
            spawnedLetters[i].gameObject.SetActive(i <= index);
        }
    }

    private void HideAllLetters()
    {
        RevealLettersUpToIndex(-1);
    }

    private IEnumerator PlayMessageSequence(string message)
    {
        PlayingMessage = true;
        EndOfVisibleCharacters = false;

        string unformattedMessage = DialogueParser.GetUnformattedMessage(message);
        string shownUnformatted = "";

        InstantiateLetters(unformattedMessage);

        int parsingIndex = 0;
        int letterIndex = 0;
        while (message.Length > parsingIndex)
        {
            string dialogueCode = DialogueParser.GetDialogueCode(message, parsingIndex);
            if (!string.IsNullOrEmpty(dialogueCode))
            {
                parsingIndex += dialogueCode.Length;
                yield return ConsumeCode(dialogueCode);
            }
            else
            {
                if (message[parsingIndex] != ' ')
                {
                    DisplayCharacter?.Invoke(message, parsingIndex);

                    RevealLettersUpToIndex(letterIndex);
                    letterIndex++;
                }
                
                shownUnformatted += message[parsingIndex];
                parsingIndex++;
                
                if (unformattedMessage == shownUnformatted)
                {
                    EndOfVisibleCharacters = true;
                }

                float adjustedFrequency = Mathf.Clamp(characterFrequency * 0.01f * DIALOGUE_SPEED, 0.01f, 0.2f);
                float waitTimer = 0f;
                while (!skipToEnd && waitTimer < adjustedFrequency)
                {
                    waitTimer += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        CompletedLine?.Invoke();
        PlayingMessage = false;
    }

    private IEnumerator ConsumeCode(string code)
    {
        if (code.StartsWith("[end"))
        {
            ForceEndLine?.Invoke();
        }
        else if (code.StartsWith("[e"))
        {
            string emotionValue = DialogueParser.GetStringValue(code, "e");
            var emotion = (Emotion)System.Enum.Parse(typeof(Emotion), emotionValue);
            EmotionChange?.Invoke(emotion);
        }
        else if (code.StartsWith("[w"))
        {
            float waitTimer = 0f;
            float waitLength = DialogueParser.GetFloatValue(code, "w");

            while (!skipToEnd && waitTimer < waitLength)
            {
                waitTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        else if (code.StartsWith("[t"))
        {
            characterFrequency = DialogueParser.GetFloatValue(code, "t");
            if (characterFrequency == 0f)
            {
                characterFrequency = DEFAULT_FREQUENCY;
            }
        }
        else if (code.StartsWith("[c"))
        {
            currentColor = DialogueParser.GetColorFromCode(code, defaultColor);
        }
        else if (code.StartsWith("[shake:"))
        {
            float intensity = DialogueParser.GetFloatValue(code, "shake");
            Shake?.Invoke(intensity);
        }
        else if (code.StartsWith("[anim:"))
        {
            string trigger = DialogueParser.GetStringValue(code, "anim");
            Animation?.Invoke(trigger);
        }
        else if (code.StartsWith("[p"))
        {
            VoicePitchAdjust = DialogueParser.GetFloatValue(code, "p");
        }
    }
}

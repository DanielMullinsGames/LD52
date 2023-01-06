using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueParser
{
    public static string ParseDialogueCodes(string message, string[] variableStrings = null)
    {
        string transformedMessage = message;
        bool openColorCode = false;

        int index = 0;
        while (message.Length > index)
        {
            string dialogueCode = GetDialogueCode(message, index);
            if (!string.IsNullOrEmpty(dialogueCode))
            {
                index += dialogueCode.Length;

                string replacementText = "";

                if (dialogueCode.StartsWith("[c"))
                {
                    string colorValue = GetStringValue(dialogueCode, "c");
                    if (colorValue == "")
                    {
                        openColorCode = false;
                        replacementText = "</color>";
                    }
                    else
                    {
                        openColorCode = true;
                        replacementText = "<color=#" + ColorUtility.ToHtmlStringRGB(GetColorFromCode(dialogueCode, Color.black)) + ">";
                    }
                }
                if (dialogueCode.StartsWith("[v"))
                {
                    int intVal = GetIntValue(dialogueCode, "v");
                    if (variableStrings != null && variableStrings.Length > 0)
                    {
                        int stringIndex = Mathf.Clamp(intVal, 0, variableStrings.Length - 1);
                        replacementText = variableStrings[stringIndex];
                    }
                }
                if (dialogueCode.StartsWith("[size"))
                {
                    int intVal = GetIntValue(dialogueCode, "size");
                    if (intVal == 0)
                    {
                        replacementText = "</size>";
                    }
                    else
                    {
                        replacementText = "<size=" + intVal + ">";
                    }
                }

                transformedMessage = transformedMessage.Replace(dialogueCode, replacementText);
            }
            else
            {
                index++;
            }
        }

        if (openColorCode)
        {
            transformedMessage += "</color>";
        }

        return transformedMessage;
    }

    public static Color GetColorFromCode(string code, Color defaultColor)
    {
        string colorValue = GetStringValue(code, "c");
        switch (colorValue)
        {
            default:
                return defaultColor;
        }
    }

    public static string GetDialogueCode(string message, int currentIndex)
	{
		string code = "";
		string currentMessage = message.Remove (0, currentIndex);
		if (message.Length > 0 && currentMessage[0] == "["[0])
		{
			if (currentMessage.IndexOf("]") +1 < currentMessage.Length)
			{
				code = currentMessage.Remove (currentMessage.IndexOf ("]") +1);
			}
			else 
			{
				code = currentMessage;
			}
		}

		return code;
	}

	public static string GetUnformattedMessage(string formattedMessage, string startDelimiter = "[", string endDelimiter = "]")
	{
		string unformatted = formattedMessage;

		while (unformatted.Contains(startDelimiter) && unformatted.Contains(endDelimiter))
		{
			int codeStartIndex = unformatted.IndexOf (startDelimiter);
			int codeEndIndex = unformatted.IndexOf (endDelimiter);
            string codeContents = unformatted.Substring(codeStartIndex, codeEndIndex - codeStartIndex);
            if (codeContents.Contains("-"))
            {
                unformatted = unformatted.Insert(codeEndIndex+1, "-");
            }

			unformatted = unformatted.Remove (codeStartIndex, codeEndIndex - codeStartIndex + 1);
		}

		return unformatted;
	}

	public static string RemoveLeadingAndTrailingDialogueCodes(string formattedMessage)
	{
		string stripped = formattedMessage;

		stripped = stripped.TrimStart (' ', '\n');
		stripped = stripped.TrimEnd (' ', '\n');

		while (stripped.Length > 0 && stripped.IndexOf ("[") == 0)
		{
			int codeEndIndex = stripped.IndexOf ("]");
			stripped = stripped.Remove (0, codeEndIndex + 1);
		}
		while (stripped.Length > 0 && stripped.LastIndexOf ("]") == stripped.Length - 1)
		{
			int codeStartIndex = stripped.LastIndexOf ("[");
			stripped = stripped.Remove (codeStartIndex, stripped.Length - codeStartIndex);
		}

		return stripped;
	}

    public static int GetIntValue(string code, string codeId)
    {
        string strippedCode = code.Replace("[", "").Replace("]", "");
        if (strippedCode[0] == codeId[0])
        {
            string intValueString = strippedCode.Remove(0, strippedCode.IndexOf(":") + 1);
            int intValue = 0;
            int.TryParse(intValueString, out intValue);
            return intValue;
        }

        return 0;
    }

    public static float GetFloatValue(string code, string codeId)
	{
		string strippedCode = code.Replace ("[","").Replace ("]","");
		if (strippedCode[0] == codeId[0])
		{
			string floatValueString = strippedCode.Remove (0, strippedCode.IndexOf (":") + 1);
			float.TryParse (floatValueString, 
                System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture,
                out float floatValue);
			return floatValue;
		}

		return 0f;
	}

	public static string GetStringValue(string code, string codeId)
	{
		string strippedCode = code.Replace ("[","").Replace ("]","");
		if (strippedCode[0] == codeId[0])
		{
			string valueString = strippedCode.Remove (0, strippedCode.IndexOf (":") + 1);
			return valueString;
		}

		return "";
	}
}

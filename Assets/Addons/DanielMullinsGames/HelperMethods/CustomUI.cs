using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomUI
{
    public static Dictionary<Vector3, char> GetTextLetterPositions(Text text)
    {
        var letterPositions = new Dictionary<Vector3, char>();

        var textGen = text.cachedTextGenerator;
        int quadIndex = 0;
        for (int i = 0; i < text.text.Length; i++)
        {
            if (text.text[i] != ' ')
            {
                int vertIndex = quadIndex * 4;
                var quadCenterPos = (textGen.verts[vertIndex].position +
                    textGen.verts[vertIndex + 1].position +
                    textGen.verts[vertIndex + 2].position +
                    textGen.verts[vertIndex + 3].position) / 4f;

                letterPositions.Add(text.transform.TransformPoint(quadCenterPos), text.text[i]);
                quadIndex++;
            }
        }

        return letterPositions;
    }
}

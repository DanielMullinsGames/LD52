using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRandom
{

    public static float ZeroCenteredValue
    {
        get
        {
            return Random.value + -0.5f;
        }
    }

    public static float RandomBetween(float min, float max)
    {
        return min + ((max - min) * Random.value);
    }

    public static bool Bool()
    {
        return Random.Range(0, 2) == 0;
    }

    public static float Sign()
    {
        return Bool() ? 1f : -1f;
    }
}

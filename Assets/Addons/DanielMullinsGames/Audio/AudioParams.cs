using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioParams
{
    [System.Serializable]
    public class Pitch
    {
        public enum Variation
        {
            Small,
            Medium,
            Large,
            VerySmall,
        }

        public float pitch;

        public Pitch(float exact)
        {
            pitch = exact;
        }

        public Pitch(float minRandom, float maxRandom)
        {
            pitch = Random.Range(minRandom, maxRandom);
        }

        public Pitch(Variation randomVariation)
        {
            switch (randomVariation)
            {
                case Variation.VerySmall:
                    pitch = Random.Range(0.95f, 1.05f);
                    break;
                case Variation.Small:
                    pitch = Random.Range(0.9f, 1.1f);
                    break;
                case Variation.Medium:
                    pitch = Random.Range(0.75f, 1.25f);
                    break;
                case Variation.Large:
                    pitch = Random.Range(0.5f, 1.5f);
                    break;
            }
        }
    }

    [System.Serializable]
    public class Repetition
    {
        public float minRepetitionFrequency;
        public string entryId;

        public Repetition(float minRepetitionFrequency, string entryId = "")
        {
            this.minRepetitionFrequency = minRepetitionFrequency;
            this.entryId = entryId;
        }
    }

    [System.Serializable]
    public class Randomization
    {
        public bool noRepeating;

        public Randomization(bool noRepeating = true)
        {
            this.noRepeating = noRepeating;
        }
    }

    [System.Serializable]
    public class Distortion
    {
        public bool muffled;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RulerView : ManagedBehaviour
{
    [SerializeField]
    private List<Sprite> mouthSprites;
    
    [SerializeField]
    private List<Sprite> eyeSprites;

    [SerializeField]
    private List<Sprite> hairSprites;

    [SerializeField]
    private List<Sprite> beardSprites;

    [SerializeField]
    private Color startSkinColor1;

    [SerializeField]
    private Color startSkinColor2;

    [SerializeField]
    private Color endSkinColor1;

    [SerializeField]
    private Color endSkinColor2;

    [SerializeField]
    private Color startHairColor;

    [SerializeField]
    private Color endHairColor;

    [SerializeField]
    private float startHeight;

    [SerializeField]
    private float maxHeightChange;

    [SerializeField]
    private AnimationCurve heightCurve;

    [SerializeField]
    private List<string> namePrefixes = new List<string>();

    [SerializeField]
    private List<string> nameSuffixes = new List<string>();

    [SerializeField]
    private bool numericalNames;

    [FoldoutGroup("Refs"), SerializeField]
    private PixelText nameText;

    [FoldoutGroup("Refs"), SerializeField]
    private PixelText ageText;

    [FoldoutGroup("Refs"), SerializeField]
    private Transform rulerParent;

    [FoldoutGroup("Refs"), SerializeField]
    private SpriteRenderer mouth;

    [FoldoutGroup("Refs"), SerializeField]
    private SpriteRenderer eyes;

    [FoldoutGroup("Refs"), SerializeField]
    private SpriteRenderer agelines;

    [FoldoutGroup("Refs"), SerializeField]
    private SpriteRenderer hair;

    [FoldoutGroup("Refs"), SerializeField]
    private SpriteRenderer beard;

    [FoldoutGroup("Refs"), SerializeField]
    private SpriteRenderer face;

    private Ruler currentRuler;
    private const float MAX_AGE = 80f;
    private float prevAge;

    private class Ruler
    {
        public float age;
        public float skinColor;
        public float adultHeight;
        public string name;
        public int nameNumber;
    }

    private void Start()
    {
        currentRuler = NewRuler(null);
    }

    public void TickAge(float timeStep)
    {
        if (currentRuler == null)
        {
            return;
        }

        currentRuler.age += timeStep * 5f;

        float deathChance = 0f;
        if (prevAge < 30f && currentRuler.age >= 30f)
        {
            deathChance = 0.2f;
        }
        else if (prevAge < 40f && currentRuler.age >= 40f)
        {
            deathChance = 0.4f;
        }
        else if (prevAge < 50f && currentRuler.age >= 50f)
        {
            deathChance = 0.6f;
        }
        else if (prevAge < 60f && currentRuler.age >= 60f)
        {
            deathChance = 0.8f;
        }
        else if (prevAge < 70f && currentRuler.age >= 70f)
        {
            deathChance = 0.9f;
        }
        else if (prevAge < 80f && currentRuler.age >= 80f)
        {
            deathChance = 1f;
        }

        if (Random.value < deathChance)
        {
            currentRuler = NewRuler(Random.value > 0.25f ? currentRuler : null);
        }

        UpdateAppearance(currentRuler);
        prevAge = currentRuler.age;

    }

    private void UpdateAppearance(Ruler ruler)
    {
        float ageNormalized = ruler.age / MAX_AGE;

        var startSkinColor = Color.Lerp(startSkinColor1, startSkinColor2, ruler.skinColor);
        var endSkinColor = Color.Lerp(endSkinColor1, endSkinColor2, ruler.skinColor);
        face.color = Color.Lerp(startSkinColor, endSkinColor, ageNormalized);

        beard.color = hair.color = Color.Lerp(startHairColor, endHairColor, Mathf.Max(0f, (ageNormalized - 0.25f) / 0.75f));

        beard.enabled = ruler.age > 14f;

        agelines.color = new Color(agelines.color.r, agelines.color.g, agelines.color.b, Mathf.Max(0f, (ageNormalized - 0.25f) / 0.75f));

        float yPos = startHeight + heightCurve.Evaluate(ageNormalized) * maxHeightChange;
        rulerParent.transform.localPosition = new Vector2(rulerParent.transform.localPosition.x, yPos);

        ageText.SetText($"{ruler.age.ToString("0")} years");
    }

    private Ruler NewRuler(Ruler parent)
    {
        var ruler = new Ruler();

        ruler.age = 10f;
        ruler.skinColor = Random.value;
        ruler.adultHeight = Random.value;
        ruler.name = parent != null ? parent.name : GenerateNewName();
        ruler.nameNumber = parent == null ? 1 : parent.nameNumber + 1;

        mouth.sprite = mouthSprites[Random.Range(0, mouthSprites.Count)];
        eyes.sprite = eyeSprites[Random.Range(0, eyeSprites.Count)];
        hair.sprite = hairSprites[Random.Range(0, hairSprites.Count)];
        beard.sprite = beardSprites[Random.Range(0, beardSprites.Count)];

        string name = ruler.name + (numericalNames && ruler.nameNumber > 1 ? " " + ToNumeral(ruler.nameNumber) : "");
        nameText.SetText(name);

        UpdateAppearance(ruler);

        return ruler;
    }

    private string GenerateNewName()
    {
        return namePrefixes[Random.Range(0, namePrefixes.Count)] + nameSuffixes[Random.Range(0, nameSuffixes.Count)];
    }

    private string ToNumeral(int num)
    {
        switch (num)
        {
            default:
                return "";
            case 2:
                return "II";
            case 3:
                return "III";
            case 4:
                return "IV";
            case 5:
                return "V";
            case 6:
                return "VI";
            case 7:
                return "VII";
            case 8:
                return "IIX";
            case 9:
                return "IX";
            case 10:
                return "X";
        }
    }
}

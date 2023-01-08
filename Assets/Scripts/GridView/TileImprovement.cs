using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileImprovementType
{
    None,
    Farm,
    Church,
    School,
}

public class TileImprovement : ManagedBehaviour
{
    public TileImprovementType ImprovementType => improvementType;
    [SerializeField]
    private TileImprovementType improvementType;

    public SpriteRenderer sr;

    public List<CivType> civs = new List<CivType>();
    public List<Sprite> sprites = new List<Sprite>();

    public void SetCiv(CivType civ)
    {
        if (civs.Contains(civ))
        {
            int index = civs.IndexOf(civ);
            if (index < sprites.Count && sr != null)
            {
                sr.sprite = sprites[index];
            }
        }
    }
}

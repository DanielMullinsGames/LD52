using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileImprovementType
{
    None,
    Farm,
}

public class TileImprovement : ManagedBehaviour
{
    public TileImprovementType ImprovementType => improvementType;
    [SerializeField]
    private TileImprovementType improvementType;
}

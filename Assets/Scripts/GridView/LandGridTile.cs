using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGridTile : ManagedBehaviour
{
    public int xCoord;
    public int yCoord;

    public TileImprovementType Improvement { get; private set; }

    public void AddImprovement(TileImprovementType improvementType)
    {
        Improvement = improvementType;
        var prefab = GameDataReferences.GetTimeImprovementPrefab(improvementType);
        var obj = Instantiate(prefab, transform);
        obj.transform.localPosition = Vector2.zero;
    }
}

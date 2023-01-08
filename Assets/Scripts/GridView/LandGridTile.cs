using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGridTile : ManagedBehaviour
{
    public int xCoord;
    public int yCoord;

    public TileImprovementType Improvement { get; private set; }

    public void AddImprovement(TileImprovementType improvementType, CivType civ)
    {
        Improvement = improvementType;
        var prefab = GameDataReferences.GetTileImprovementPrefab(improvementType);
        var obj = Instantiate(prefab, transform);
        obj.transform.localPosition = Vector2.zero;

        obj.GetComponent<TileImprovement>().SetCiv(civ);

        ScreenShake.instance.AddIntensity(3f);
    }
}

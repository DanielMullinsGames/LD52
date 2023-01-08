using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LandGrid : ManagedBehaviour
{
    [SerializeField]
    private List<LandGridTile> tiles = new List<LandGridTile>();

    [Title("Grid Generation")]
    [SerializeField]
    private GameObject tilePrefab = default;

    [SerializeField]
    private float gridSpacing = default;

    [SerializeField]
    private Vector2 anchorPos = default;

    private const int GRID_SIZE_X = 9;
    private const int GRID_SIZE_Y = 7;

    [Button("Generate Grid")]
    public void GenerateGrid()
    {
        tiles.Clear();
        for (int x = 0; x < GRID_SIZE_X; x++)
        {
            for (int y = 0; y < GRID_SIZE_Y; y++)
            {
                var obj = UnityEditor.PrefabUtility.InstantiatePrefab(tilePrefab) as GameObject;
                obj.transform.parent = transform;
                obj.transform.localPosition = anchorPos + new Vector2(gridSpacing * x, gridSpacing * y);
                obj.name = $"Tile ({x},{y})";

                var tile = obj.GetComponent<LandGridTile>();
                tile.xCoord = x;
                tile.yCoord = y;
                tiles.Add(tile);
            }
        }
    }

    public void AddImprovementToRandomTile(TileImprovementType improvementType)
    {
        var openTiles = tiles.FindAll(x => x.gameObject.activeSelf && x.Improvement == TileImprovementType.None);
        openTiles[Random.Range(0, openTiles.Count)].AddImprovement(improvementType);
    }

    public int GetNumImprovementsOfType(TileImprovementType improvement)
    {
        return tiles.FindAll(x => x.Improvement == improvement).Count;
    }
}

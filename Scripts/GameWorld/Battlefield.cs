using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct BattlefieldArea
{
    public Vector3 center;
    public float radius;

    public Vector4
}

public class Battlefield
{
    private List<BattlefieldArea> areas;

    public HashSet<HexTile> tiles { get; private set; }

    public Vector4[] maskArray { get; private set; }

    public int maskLength { get; private set; }

    public Battlefield()
    {
        tiles = new HashSet<HexTile>();
        maskArray = new Vector4[10];
    }

    public void AddArea(HexTile center, int range)
    {
        List<HexTile> tiles = HexTerrain.GetTilesInRange(center, range, true);

        foreach (HexTile tile in tiles)
        {
            tiles.Add(tile);
        }

        areas.Add(new BattlefieldArea()
        {
            center = center.position,
            radius = range * HexTerrain.tileSize
        });

        int i = 0;

        foreach (BattlefieldArea area in areas)
        {
            maskArray[i++] = area.mask;
        }

        maskLength = i;
    }
}

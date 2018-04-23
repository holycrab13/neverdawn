using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugPathfinder : MonoBehaviour {

    public HexTile start;

    public HexTile end;

    private List<HexTile> path;

	void OnValidate()
    {
        if(start != null && end != null) 
            path = Pathfinder.FindPath(start, end);
    }

    void OnDrawGizmos()
    {
        if (path != null)
        {
            foreach (HexTile tile in path)
            {
                Gizmos.DrawWireSphere(tile.position, 0.1f);
            }
        }
    }
}

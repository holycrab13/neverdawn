using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum PathFindingMode
{
    None,
    ExcludeTarget,
    NoBlocking
}

class Pathfinder
{
    private static List<HexTile> openList;

    private static List<HexTile> closedList;

    private static Dictionary<HexTile, float> f;

    private static Dictionary<HexTile, float> g;

    private static Dictionary<HexTile, HexTile> previous;

    private static HexTile endNode;

    private static HexTile startNode;
    private static Frame targetFrame;
    private static PathFindingMode pathFindingMode;

    public static int Distance(HexTile start, HexTile end)
    {
        List<HexTile> path = FindPath(start, end, PathFindingMode.NoBlocking);

        if (path != null)
        {
            return path.Count - 1;
        }

        return -1;
    }


    public static List<HexTile> FindPath(HexTile start, HexTile end, PathFindingMode mode = PathFindingMode.None)
    {
        endNode = end;
        startNode = start;
        pathFindingMode = mode;

        if (pathFindingMode == PathFindingMode.ExcludeTarget)
        {
            targetFrame = endNode.currentFrame;
        }

        f = new Dictionary<HexTile, float>();
        g = new Dictionary<HexTile, float>();
        previous = new Dictionary<HexTile, HexTile>();

        openList = new List<HexTile>();
        closedList = new List<HexTile>();

        f[startNode] = 0.0f;
        openList.Add(start);

        while(openList.Count > 0)
        {
            HexTile current = popMinTile();

            if (current == end)
            {
                return createPath();
            }

            closedList.Add(current);

            epxandNode(current);
        }

        // No path found!
        return null;
    }

    private static List<HexTile> createPath()
    {
        List<HexTile> result = new List<HexTile>();

        HexTile current = endNode;

        while (current != startNode)
        {
            result.Insert(0, current);

            current = previous[current];
        }

        result.Insert(0, startNode);

        return result;
    }

    private static HexTile popMinTile()
    {
        float min = float.MaxValue;
        HexTile minTile = null;

        foreach(HexTile tile in openList)
        {
            if (f[tile] < min)
            {
                minTile = tile;
                min = f[tile];
            }
        }

        openList.Remove(minTile);
        return minTile;
    }

    private static void epxandNode(HexTile current)
    {
        foreach(HexPath path in current.neighbors)
        {
            HexTile successor = path.target;

            if (pathFindingMode != PathFindingMode.NoBlocking)
            {
                if (successor.isOccluded || successor.type == HexTileType.Disabled
                    || !(successor.currentFrame == null || (successor.currentFrame == targetFrame && pathFindingMode == PathFindingMode.ExcludeTarget)))
                    continue;
            }

            if(closedList.Contains(successor))
            {
                continue;
            }

            float tentative_g = getG(current) + Vector3.Distance(current.position, successor.position);
            
            if (openList.Contains(successor) && tentative_g >= getG(successor))
            {
                continue;
            }

            previous[successor] = current;
            g[successor] = tentative_g;

            f[successor] = tentative_g + heuristic(successor);

            if (!openList.Contains(successor))
            {
                openList.Add(successor);
            }
        }
    }

    private static float getG(HexTile current)
    {
        return g.ContainsKey(current) ? g[current] : 0.0f;

    }

    private static float heuristic(HexTile successor)
    {
        return Vector3.Distance(successor.position, endNode.position);
    }
}

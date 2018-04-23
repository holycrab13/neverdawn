using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using nAlpha;

public class HexOutline : MonoBehaviour
{
    public int handle;
    public List<HexTile> tiles;
    public Color color;
    public Material material;
    public float thickness;

    void Start()
    {

        material = Instantiate(material);
        material.renderQueue = 2001;
        material.color = color;

        List<Tuple<Vector3, Vector3>> edges = new List<Tuple<Vector3, Vector3>>();

        foreach (HexTile tile in tiles)
        {
            if (tile.isOccluded || tile.type == HexTileType.Disabled)
                continue;

            foreach (HexPath path in tile.neighbors)
            {
                // path is crossing a border
                if (path.target.type == HexTileType.Disabled || path.target.isOccluded || !tiles.Contains(path.target))
                {
                    LineHexagon lineHex = tile.GetComponent<LineHexagon>();
                    Vector3[] corners = lineHex.worldSpaceCorners;

                    edges.Add(new Tuple<Vector3, Vector3>(corners[path.index], corners[(path.index + 1) % 6]));
                }
            }
        }

        List<Vector3[]> lineStrips = CreateLineStrips(edges);

        foreach (Vector3[] lineStrip in lineStrips)
        {


            GameObject stripObject = new GameObject("outline");
            stripObject.transform.SetParent(transform);
            stripObject.layer = 14;

            MeshFilter filter = stripObject.AddComponent<MeshFilter>();
            MeshRenderer renderer = stripObject.AddComponent<MeshRenderer>();
            renderer.material = material;

            Mesh mesh = new Mesh();
            mesh.vertices = HexHelper.CreateLineStripVertices(lineStrip, thickness / 2.0f);
            mesh.SetIndices(HexHelper.CreateLineStripIndices(lineStrip.Length, true), MeshTopology.Triangles, 0);

            filter.mesh = mesh;
        }
    }

    private bool isSameVertex(Vector3 v1, Vector3 v2)
    {
        return (v1 - v2).sqrMagnitude < 0.1f;
    }

    private List<Vector3[]> CreateLineStrips(List<Tuple<Vector3, Vector3>> edges)
    {
        List<Vector3[]> result = new List<Vector3[]>();

        while (edges.Count > 0)
        {
            List<Vector3> currentShape = new List<Vector3>();

            Vector3 start = edges[0].Item1;
            Vector3 end = edges[0].Item2;
            edges.RemoveAt(0);

            currentShape.Add(start);

            while (!isSameVertex(start, end))
            {
                currentShape.Add(end);

                Tuple<Vector3, Vector3> nextEdge = edges.Find(v => isSameVertex(end, v.Item1));
                edges.Remove(nextEdge);

                end = nextEdge.Item2;
            }

            result.Add(currentShape.ToArray());
        }

        return result;
    }

}
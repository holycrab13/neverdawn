using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexHelper
{
    public static int[] CreateLineStripIndices(int length, bool loop)
    {
        int segmentCount = loop ? length : (length - 1);

        int[] result = new int[segmentCount * 6];

        int k = 0; 

        for(int i = 0; i < segmentCount; i++)
        {
            result[k++] = i;
            result[k++] = i + length;
            result[k++] = (i + 1) % length;
            result[k++] = (i + 1) % length;
            result[k++] = i + length;
            result[k++] = length + ((i + 1) % length);
        }

        return result;
    }

    public static Vector3[] getCorners(float width)
    {
        return corners.Select(c => width * c).ToArray();
    }

    public static Vector3[] corners = {
		new Vector3(0f, 0f, (1.0f / 0.866025404f)),
		new Vector3(1.0f, 0f, 0.5f * (1.0f / 0.866025404f)),
		new Vector3(1.0f, 0f, -0.5f * (1.0f / 0.866025404f)),
		new Vector3(0f, 0f, -(1.0f / 0.866025404f)),
		new Vector3(-1.0f, 0f, -0.5f * (1.0f / 0.866025404f)),
		new Vector3(-1.0f, 0f, 0.5f * (1.0f / 0.866025404f))
	};

    internal static Vector3[] CreateLineStripVertices(Vector3[] lineStrip, float thickness)
    {
        Vector3[] result = new Vector3[lineStrip.Length * 2];

        for (int i = 0; i < lineStrip.Length; i ++)
        {
            result[i] = shift(lineStrip, i, thickness);
        }

        for (int i = 0; i < lineStrip.Length; i++)
        {
            result[i + lineStrip.Length] = shift(lineStrip, i, -thickness);
        }

        return result;
    }

    private static Vector3 shift(Vector3[] lineStrip, int p, float thickness)
    {
        int prev = NeverdawnUtility.RepeatIndex(p - 1, lineStrip.Length);
        int next = NeverdawnUtility.RepeatIndex(p + 1, lineStrip.Length);
        Vector3 shiftDirection = lineStrip[next] - lineStrip[prev];
        shiftDirection.y = 0.0f;
        shiftDirection.Normalize();
        shiftDirection = Quaternion.Euler(0.0f, 90.0f, 0.0f) * shiftDirection;

        return lineStrip[p] + thickness * shiftDirection;
    }
}

[RequireComponent(typeof(MeshFilter))]
public class LineHexagon : MonoBehaviour {

    private Mesh mesh;

    private MeshFilter filter;

    public float thickness;

    public float scale;

	// Use this for initialization
	public void CreateMesh() {

        filter = GetComponent<MeshFilter>();

        mesh = new Mesh();
        mesh.vertices = HexHelper.CreateLineStripVertices(HexHelper.getCorners(.5f * scale), thickness / 2.0f);
        mesh.SetIndices(HexHelper.CreateLineStripIndices(6, true), MeshTopology.Triangles, 0);

        filter.mesh = mesh;
	}


    public Vector3[] worldSpaceCorners
    {
        get
        {
            return HexHelper.getCorners(.5f * scale).Select(c => transform.TransformPoint(c)).Select(v => new Vector3(v.x, 0.0f, v.z)).ToArray();
        }
    }
}

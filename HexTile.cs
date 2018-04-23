using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class HexMetrics
{
    public const float outerRadius = 1f;
    public const float innerRadius = outerRadius * 0.866025404f;
    public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
	};
}

public enum HexTileType
{
    Empty,
    StaticInteractable,
    Disabled
}



[Serializable]
public class HexPath
{
    public HexTile target;

    public int index;

    public HexPath(HexTile hexTile)
    {
        this.target = hexTile;
    }
}

[ExecuteInEditMode]
public class HexTile : MonoBehaviour
{

    public List<HexPath> neighbors;

    [SerializeField]
    private Frame _currentFrame;

    [SerializeField]
    private HexTileType _type;

    [SerializeField]
    [HideInInspector]
    private bool _isOccluded;

    private int highlighterCount;

    [SerializeField]
    private Color highlightColor;


    [SerializeField]
    private SpriteRenderer outlineRenderer;

    public float fValue;
    public int x;
    public int y;

    public int index;


    internal void AddPath(HexTile hexTile)
    {
        HexPath path = new HexPath(hexTile);
       
        Vector3 direction = hexTile.position - position;
        direction.y = 0.0f;

        float angle = Vector3.SignedAngle(Vector3.back, direction, Vector3.up) + 150.0f;
        path.index = Mathf.RoundToInt(angle / 60.0f);

        neighbors.Add(path);

    }

    void OnValidate()
    {
        if (isOccluded || type == HexTileType.Disabled)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);

            switch (_type)
            {
                case HexTileType.Empty:
                    break;
                case HexTileType.StaticInteractable:
                    break;
            }
        }

    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawIcon((transform.position + Vector3.up * (height + 1.0f), 0.1f);
    }

    public bool isOccluded
    {
        get { return _isOccluded; }
        set
        {
            _isOccluded = value; 
            OnValidate();
        }
    }

    public HexTileType type
    {
        get { return _type; }
        set
        {
            _type = value;
            OnValidate();
        }
    }


    public Frame currentFrame
    {
        get { return _currentFrame; }
        set { _currentFrame = value; }
    }


    public Vector3 position
    {
        get { return transform.position; }
    }

    internal bool IsAdjacent(HexTile hexTile)
    {
        foreach (HexPath path in neighbors)
        {
            if (path.target == hexTile)
            {
                return true;
            }
        }

        return false;
    }

    [SerializeField]
    private Color defaultColor;

    internal void Highlight()
    {
        Highlight(defaultColor);
    }

    internal void Highlight(Color color)
    {
        highlightColor = color;
        highlighterCount++;

        OnValidate();
    }

    internal void Unlight(float delay = 0.0f)
    {
        StartCoroutine(UnlightWithDelay(delay));
    }

    IEnumerator UnlightWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        highlighterCount--;
        OnValidate();
    }
}

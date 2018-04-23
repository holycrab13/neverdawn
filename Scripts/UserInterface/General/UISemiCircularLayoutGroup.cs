using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISemiCircularLayoutGroup : LayoutGroup
{

    public float childSize;

    [SerializeField]
    private float arc;

    private Vector2 centerPoint;

    protected override void OnEnable()
    {
        RectTransform f = transform.GetComponent<RectTransform>();
        base.OnEnable();
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        CalculateLayoutInputForAxis(0);
    }

    public override void CalculateLayoutInputVertical()
    {
        CalculateLayoutInputForAxis(1);
    }

    public override void SetLayoutHorizontal()
    {
        SetLayoutAlongAxis(0);
    }

    public override void SetLayoutVertical()
    {
        SetLayoutAlongAxis(1);
    }

    void SetLayoutAlongAxis(int axis)
    {
        // Take all the space, except the padding.
       
        // Everybody starts at the same place.
        RectTransform rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.rect.size.x;
        float height = rectTransform.rect.size.y;

        //float pos = GetStartOffset(axis, 0);
        float dAngle = arc / (rectChildren.Count - 1);
        // Overlap all the things.

        // position of first element
        Vector2 firstPos = new Vector2(padding.left, 0.0f);
        Vector2 lastPos = new Vector2(width - padding.right - childSize, 0.0f);

        float baseAngle = Mathf.Deg2Rad * ((180.0f - arc) / 2.0f);
        float L = (lastPos - firstPos).magnitude / 2.0f;
        float d = L / Mathf.Cos(baseAngle);

        centerPoint = new Vector2(padding.left + L, height - padding.bottom + d * Mathf.Sin(baseAngle));

        for (int i = 0; i < rectChildren.Count; i++)
        {
            float angle = (i * dAngle - arc / 2.0f) * Mathf.Deg2Rad;
            float y = centerPoint.y - (d * Mathf.Cos(angle)) - childSize;
            float x = centerPoint.x - (d * Mathf.Sin(angle));

            float pos = (axis == 0 ? x : y);
            RectTransform child = rectChildren[i];
            
            SetChildAlongAxis(child, axis, pos, childSize);
        }

    }

    void CalculateLayoutInputForAxis(int axis)
    {
        // We need to reserve space for the padding.
        float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);

        float totalmin = combinedPadding + 2.0f + childSize;
        float totalpreferred = combinedPadding + 2.0f + childSize;

        // We ignore flexible size for now, I have not decided what to do with it yet.
        SetLayoutInputForAxis(totalmin, totalpreferred, 1, axis);
    }
}

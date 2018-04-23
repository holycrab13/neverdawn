using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICircularLayoutGroup : LayoutGroup
{
    public float radius;

    public float childSize;

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
        RectTransform rect = GetComponent<RectTransform>();

        //float pos = GetStartOffset(axis, 0);
        float dAngle = 360.0f / rectChildren.Count;
        // Overlap all the things.
        for (int i = 0; i < rectChildren.Count; i++)
        {

            float y = rect.sizeDelta.x / 2.0f - childSize / 2.0f - (radius * Mathf.Cos(i * dAngle * Mathf.PI / 180.0f));
            float x = rect.sizeDelta.y / 2.0f - childSize / 2.0f + (radius * Mathf.Sin(i * dAngle * Mathf.PI / 180.0f));

            float pos = (axis == 0 ? x : y);
            RectTransform child = rectChildren[i];
            
            SetChildAlongAxis(child, axis, pos, childSize);
        }
    }

    void CalculateLayoutInputForAxis(int axis)
    {
        // We need to reserve space for the padding.
        float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);

        float totalmin = combinedPadding + 2.0f * radius + childSize;
        float totalpreferred = combinedPadding + 2.0f * radius + childSize;

        // We ignore flexible size for now, I have not decided what to do with it yet.
        SetLayoutInputForAxis(totalmin, totalpreferred, 1, axis);
    }
}

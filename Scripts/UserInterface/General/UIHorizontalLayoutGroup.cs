using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHorizontalLayoutGroup : LayoutGroup
{
    [SerializeField]
    private int spacing;

    [SerializeField]
    private int minColumnCount;

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
        // SetLayoutAlongAxis(1);
    }

    void SetLayoutAlongAxis(int axis)
    {
        // Take all the space, except the padding.
       
        // Everybody starts at the same place.
        int childCount = Mathf.Max(minColumnCount, rectChildren.Count);
        RectTransform rect = GetComponent<RectTransform>();
        float totalWidth = rect.sizeDelta.x;
        float componentsWidth = totalWidth - padding.left - padding.right - (childCount - 1) * spacing;

        float dX = componentsWidth / childCount;

        float size = (axis == 0 ? dX : 0.0f);

        if (rectChildren.Count == 2)
        {
            float pos0 = (axis == 0 ? padding.left : 0.0f);
            SetChildAlongAxis(rectChildren[0], axis, pos0, size);

            float pos1 = (axis == 0 ? totalWidth - padding.right - size : 0.0f);
            SetChildAlongAxis(rectChildren[1], axis, pos1, size);

        }
        else
        {
            // Overlap all the things.
            for (int i = 0; i < rectChildren.Count; i++)
            {

                float x = padding.left + dX * i + i * spacing;

                float pos = (axis == 0 ? x : 0.0f);
                RectTransform child = rectChildren[i];

                SetChildAlongAxis(child, axis, pos, size);
            }
        }
    }

    void CalculateLayoutInputForAxis(int axis)
    {
        // We need to reserve space for the padding.
        float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);

        float totalmin = combinedPadding;
        float totalpreferred = combinedPadding;

        // We ignore flexible size for now, I have not decided what to do with it yet.
        SetLayoutInputForAxis(totalmin, totalpreferred, 1, axis);
    }
}

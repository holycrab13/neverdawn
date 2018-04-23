using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectable : UIBehaviour
{
    public UISelectable neighborTop;

    public UISelectable neighborBottom;

    public UISelectable neighborLeft;

    public UISelectable neighborRight;

    public bool isSelected { get; private set; }

    protected enum SelectionState
    {
        // Summary:
        //     The UI object can be selected.
        Normal = 0,
        //
        // Summary:
        //     The UI object is highlighted.
        Highlighted = 1,
        //
        // Summary:
        //     The UI object is pressed.
        Pressed = 2,
        //
        // Summary:
        //     The UI object cannot be selected.
        Disabled = 3,
    }

    private UIInputController inputController;

    private RectTransform _rectTransform;

    public RectTransform rectTransform
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            return _rectTransform;
        }
    }

    public Rect rectangle
    {
        get { return GetWorldRect(rectTransform, Vector2.one); }
    }


    public static Rect GetWorldRect(RectTransform rt, Vector2 scale)
    {
        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        // Rescale the size appropriately based on the current Canvas scale
        Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

        return new Rect(topLeft, scaledSize);
    }

    protected virtual void Update()
    {

    }

    protected virtual void DoStateTransition(SelectionState state, bool instant)
    {
        
    }

    protected virtual void Start()
    {
        inputController = GetComponentInParent<UIInputController>();
        inputController.Register(this);
    }

    void OnDestroy()
    {
        inputController.Unregister(this);
    }

    public void Select()
    {
         if(inputController == null)
            inputController = GetComponentInParent<UIInputController>();

        if (inputController == null)
            throw new InvalidOperationException("UISelectable.Select() called without input controller");

        inputController.Select(this);
    }


    internal void Deselect()
    {
        if (inputController == null)
            inputController = GetComponentInParent<UIInputController>();

        if (inputController == null)
            throw new InvalidOperationException("UISelectable.Select() called without input controller");

        inputController.Select(null);
    }

    public virtual void OnPointerDown(UIInputEventArgs eventArgs)
    {
        DoStateTransition(SelectionState.Pressed, false);
    }

    public virtual void OnPointerUp(UIInputEventArgs eventArgs)
    {
        if (isSelected)
            DoStateTransition(SelectionState.Highlighted, false);
    }

    public virtual void OnSelect(UIInputEventArgs eventArgs)
    {
        isSelected = true;
        DoStateTransition(SelectionState.Highlighted, false);
    }

    public virtual void OnDeselect(UIInputEventArgs eventArgs)
    {
        isSelected = false;
        DoStateTransition(SelectionState.Normal, false);
    }

}

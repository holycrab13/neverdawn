using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : UISelectable
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Color iconColor = new Color(1, 1, 1, .8f);

    [SerializeField]
    private Color iconSelectColor = new Color(1, 1, 1, 1);

    [SerializeField]
    private Color iconPressedColor = new Color(1, 1, 1, .9f);

    [SerializeField]
    private Color iconDisabledColor = new Color(.5f, .5f, .5f, .8f);

    [SerializeField]
    private float iconFadeDuration = 0.2f;

    [SerializeField]
    private bool _interactable = true;

    [SerializeField]
    private UnityEvent _onClick;

    [SerializeField]
    private UnityEvent _onSelect;

    [SerializeField]
    private UnityEvent _onDeSelect;

    private SelectionState _selectionState;

    /// <summary>
    /// Set the sprite of the icon button
    /// </summary>
    /// <param name="sprite"></param>
    public void SetIconSprite(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    void OnEnable()
    {
        if (_interactable)
        {
            if(isSelected)
            {
                icon.CrossFadeColor(iconSelectColor, 0.0f, false, true);
            }
            else
            {
                icon.CrossFadeColor(iconColor, 0.0f, false, true);
            }
        }
        else
        {
            icon.CrossFadeColor(iconDisabledColor, 0.0f, false, true);
        }
    }


    /// <summary>
    /// Do a state transition
    /// </summary>
    /// <param name="state"></param>
    /// <param name="instant"></param>
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        _selectionState = state;

        if (_interactable)
        {
            switch (state)
            {
                case SelectionState.Pressed:
                    icon.CrossFadeColor(iconPressedColor, iconFadeDuration, false, true);
                    break;
                case SelectionState.Normal:
                    icon.CrossFadeColor(iconColor, iconFadeDuration, false, true);
                    break;
                case SelectionState.Highlighted:
                    icon.CrossFadeColor(iconSelectColor, iconFadeDuration, false, true);
                    break;

            }
        }
        else
        {
            switch (state)
            {
                case SelectionState.Pressed:
                    icon.CrossFadeColor(iconDisabledColor, 0.0f, false, true);
                    break;
                case SelectionState.Normal:
                    icon.CrossFadeColor(iconDisabledColor, 0.0f, false, true);
                    break;
                case SelectionState.Highlighted:
                    icon.CrossFadeColor(iconDisabledColor, 0.0f, false, true);
                    break;

            }
        }

        base.DoStateTransition(state, instant);
    }

    public override void OnPointerUp(UIInputEventArgs eventData)
    {
        if (interactable)
        {
            _onClick.Invoke();
        }

        base.OnPointerUp(eventData);
    }

    public override void OnSelect(UIInputEventArgs eventData)
    {
        _onSelect.Invoke();
        base.OnSelect(eventData);
    }

    public override void OnDeselect(UIInputEventArgs eventData)
    {
        _onDeSelect.Invoke();
        base.OnDeselect(eventData);
    }

    public UnityEvent onClick { get { return _onClick; } }

    public UnityEvent onSelect { get { return _onSelect; } }

    public UnityEvent onDeSelect { get { return _onDeSelect; } }

    public bool interactable
    {
        get { return _interactable; }
        set
        {
            _interactable = value;
            DoStateTransition(_selectionState, false);
        }
    }

}

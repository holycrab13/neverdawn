using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPage : MonoBehaviour {

    [SerializeField]
    private UISelectable defaultSelectable;

    private UIRoot _root;

    private CanvasGroup _group;

    protected UIRoot root
    {
        get
        {
            if (!_root)
            {
                _root = GetComponentInParent<UIRoot>();
            }

            return _root;
        }
    }

    public CanvasGroup group
    {
        get
        {
            if (!_group)
                _group = GetComponent<CanvasGroup>();

            return _group;
        }
    }

    public void Show()
    {
        root.ShowPage(this);
    }

    public void Hide()
    {
        root.ShowPage(null);
    }

    internal bool IsVisible()
    {
        return group.alpha > 0.0f;
    }

    public virtual void OnShow()
    {
        group.alpha = 1.0f;

        if (defaultSelectable)
            defaultSelectable.Select();
    }

    public virtual void OnHide()
    {
        group.alpha = 0.0f;
        UIInputController inputController = GetComponentInParent<UIInputController>();

        if (inputController)
            inputController.Deselect();
    }

    public virtual void OnControllerActivated(InputModule module)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIGroup : MonoBehaviour {

    private CanvasGroup _group;

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
        group.alpha = 1.0f;
        OnShow();
    }

    protected virtual void OnShow()
    {

    }

    protected virtual void OnHide()
    {

    }

    public void Hide()
    {
        group.alpha = 0.0f;
        OnHide();
    }

    internal bool IsVisible()
    {
        return group.alpha > 0.0f;
    }
}

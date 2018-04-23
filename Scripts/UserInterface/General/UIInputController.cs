using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputEventArgs
{

}

public class UIInputController : MonoBehaviour {

    public InputModule[] inputModules;

    public UISelectable selected { get; set; }

    [SerializeField]
    private List<UISelectable> selectables;

    private UISelectable pressed;

    private float prevHorizontal;

    private float prevVertical;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }

    public void SetInputModules(params InputModule[] inputModules)
    {
        this.inputModules = inputModules;
    }

	void Update () {

		if(inputModules != null && inputModules.Length > 0 && inputModules[0] != null   )
        {
            if(selected == null || !selected.gameObject.activeInHierarchy)
            {
                Select(null);
            }

            foreach (InputModule inputModule in inputModules)
            {
                if (inputModule.GetButtonDown(NeverdawnInputButton.Confirm))
                {
                    pressed = selected;

                    if (selected != null)
                    {
                        selected.OnPointerDown(null);
                    }
                }

                if (inputModule.GetButtonUp(NeverdawnInputButton.Confirm))
                {
                    if (selected != null)
                    {
                        if (selected == pressed)
                        {
                            selected.OnPointerUp(null);
                        }
                    }

                    pressed = null;
                }

                if (selected != null)
                {
                    if (inputModule.GetButtonDown(NeverdawnInputButton.Left))
                    {
                        selectLeft();
                    }

                    if (inputModule.GetButtonDown(NeverdawnInputButton.Right))
                    {
                        selectRight();
                    }


                    if (inputModule.GetAxisDown(NeverdawnInputAxis.HorizontalLeft, NeverdawnInputAxisDirection.Positive)
                        || inputModule.GetAxisDown(NeverdawnInputAxis.HorizontalDPad, NeverdawnInputAxisDirection.Positive))
                    {
                        selectRight();
                    }

                    if (inputModule.GetAxisDown(NeverdawnInputAxis.HorizontalLeft, NeverdawnInputAxisDirection.Negative)
                        || inputModule.GetAxisDown(NeverdawnInputAxis.HorizontalDPad, NeverdawnInputAxisDirection.Negative))
                    {
                        selectLeft();
                    }

                    if (inputModule.GetAxisDown(NeverdawnInputAxis.VerticalLeft, NeverdawnInputAxisDirection.Positive)
                        || inputModule.GetAxisDown(NeverdawnInputAxis.VerticalDPad, NeverdawnInputAxisDirection.Positive))
                    {
                        selectTop();
                    }

                    if (inputModule.GetAxisDown(NeverdawnInputAxis.VerticalLeft, NeverdawnInputAxisDirection.Negative)
                        || inputModule.GetAxisDown(NeverdawnInputAxis.VerticalDPad, NeverdawnInputAxisDirection.Negative))
                    {
                        selectBottom();
                    }
                }

                if (inputModule.HasCursor)
                {
                    if (selectables != null && canvasGroup.alpha > 0.0f)
                    {
                        Vector3 cursorPosition = inputModule.cursorPosition;

                        bool childSelected = false;

                        foreach (UISelectable selectable in selectables)
                        {
                            if (selectable.gameObject.activeInHierarchy && selectable.rectangle.Contains(cursorPosition))
                            {
                                childSelected = true;
                                selectable.Select();
                                break;
                            }
                        }

                        if (!childSelected)
                        {
                            Deselect();
                        }
                    }
                }
            }
        }
	}


    private void selectRight()
    {
        if (selected.neighborRight != null)
            Select(selected.neighborRight);
    }

    private void selectLeft()
    {
        if (selected.neighborLeft != null)
            Select(selected.neighborLeft);
    }

    private void selectBottom()
    {
        if (selected.neighborBottom != null)
            Select(selected.neighborBottom);
    }

    private void selectTop()
    {
        if (selected.neighborTop != null)
            Select(selected.neighborTop);
    }

    internal void Select(UISelectable selectable)
    {
        if (selected == selectable)
            return;

        if(selected != null)
        {
            selected.OnDeselect(null);
        }

        selected = selectable;   
        
        if(selected != null)
        {
            selected.OnSelect(null);
        }
    }

    internal void Deselect()
    {
        Select(null);
    }

    internal void Register(UISelectable uISelectable)
    {
        if (selectables == null)
            selectables = new List<UISelectable>();

        selectables.Add(uISelectable);
    }

    internal void Unregister(UISelectable uISelectable)
    {
        if (selectables == null)
            selectables = new List<UISelectable>();

        selectables.Remove(uISelectable);
    }
}

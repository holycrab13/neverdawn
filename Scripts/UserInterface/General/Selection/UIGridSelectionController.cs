using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayout))]
public class UIGridSelectionController : MonoBehaviour {

    private GridLayoutGroup layout;

    public  InputModule inputModule;

    private UISelectable[] selectables;

    private float prevHorizontal;

    private float prevVertical;
    private int index;
    private RectTransform rectTransform;

    [SerializeField]
    private int maxRows = 4;

    private int currentRow;
    private float targetScroll;
    private float lerp;
    private float currentScroll;

    [SerializeField]
    private float scrollSpeed;

    public UnityEvent onSelectionChanged;

    public UISelectable selected { get; private set; }

    public int selectedIndex
    {
        get { return index; }
    }



    public int length
    {
        get { return selectables != null ? selectables.Length : 0; }
    }

    void Start()
    {
        layout = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
        selectables = GetComponentsInChildren<UISelectable>();

        prevVertical = float.MaxValue;
        prevHorizontal = float.MaxValue; 
    }

    void Update()
    {
        if (inputModule != null && selectables != null)
        {
            float horizontal = inputModule.GetAxis(NeverdawnInputAxis.HorizontalLeft);
            float vertical = inputModule.GetAxis(NeverdawnInputAxis.VerticalLeft);

            if (inputModule.GetButtonDown(NeverdawnInputButton.Left))
            {
                selectLeft();
            }

            if (inputModule.GetButtonDown(NeverdawnInputButton.Right))
            {
                selectRight();
            }

            if (horizontal > 0.0f && prevHorizontal == 0.0f && Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                selectRight();
            }

            if (horizontal < 0.0f && prevHorizontal == 0.0f && Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                selectLeft();
            }

            if (vertical > 0.0f && prevVertical == 0.0f && Mathf.Abs(horizontal) < Mathf.Abs(vertical))
            {
                selectTop();
            }

            if (vertical < 0.0f && prevVertical == 0.0f && Mathf.Abs(horizontal) < Mathf.Abs(vertical))
            {
                selectBottom();
            }


            prevHorizontal = horizontal;
            prevVertical = vertical;
        }

        if(lerp <= 1.0f)
        {
            lerp += Time.deltaTime * scrollSpeed;
            rectTransform.anchoredPosition = new Vector2(0.0f, Mathf.Lerp(currentScroll, targetScroll, lerp));
        }
    }

    private void selectBottom()
    {
        int columnCount = layout.constraintCount;

        if (index + columnCount < selectables.Length)
        {
            index = index + columnCount;
        }

        updateSelection();
    }

    private void selectTop()
    {
        int columnCount = layout.constraintCount;

        if (index - columnCount >= 0)
        {
            index = index - columnCount;
        }

        updateSelection();
    }


    private void selectRight()
    {
        index = NeverdawnUtility.RepeatIndex(index + 1, selectables.Length);
        updateSelection();
    }

    private void selectLeft()
    {
        index = NeverdawnUtility.RepeatIndex(index - 1, selectables.Length);
        updateSelection();
    }


    internal void Select(int selectionIndex)
    {
        index = NeverdawnUtility.RepeatIndex(selectionIndex, selectables.Length);
        updateSelection();
    }

    private void updateSelection()
    {
        if (onSelectionChanged != null)
        {
            onSelectionChanged.Invoke();
        }

        if (selectables == null || selectables.Length == 0)
            return;

        selected = selectables[index];
        
        selectables[index].Select();

        int columnCount = layout.constraintCount;
        int rowIndex = index / columnCount;
        int tmpRow = currentRow;

        if (rowIndex < currentRow)
            currentRow = rowIndex;

        if (rowIndex - maxRows + 1 > currentRow)
            currentRow = rowIndex - maxRows + 1;

        if(tmpRow != currentRow)
        {
            currentScroll = rectTransform.anchoredPosition.y;
            targetScroll = currentRow * 80.0f;
            lerp = 0.0f;
        }
    }

    
    void OnTransformChildrenChanged()
    {
  
        selectables = GetComponentsInChildren<UISelectable>();

        
    }

}

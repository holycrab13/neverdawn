using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIQuickMenuInventory : UIQuickMenuPage
{
    [SerializeField]
    private UIQuickMenuIconButton iconButtonPrefab;

    [SerializeField]
    private RectTransform iconButtonParent;

    [SerializeField]
    private UIGridSelectionController gridController;

    [SerializeField]
    private TMP_Text itemStackLabel;

    private int selectionIndex;

    private PickableStack[] stacks;


    /// <summary>
    /// Setup grid controller and listen for item changes
    /// </summary>
    void Start()
    {
        gridController.onSelectionChanged.AddListener(selectionChanged);
        gridController.inputModule = avatarController.inputModule;

        PlayerInventory.collection.onItemChanged += NeverdawnInventory_onItemChanged;

        updateView();
    }

    void OnEnable()
    {
        Invoke("updateSelection", 0.0f);
    }

    /// <summary>
    /// Remove hook to itemChanged event
    /// </summary>
    protected override void OnDestroy()
    {
        PlayerInventory.collection.onItemChanged -= NeverdawnInventory_onItemChanged;
        base.OnDestroy();
    }

    void NeverdawnInventory_onItemChanged(Pickable pickable)
    {
        updateView();
    }

    private void updateView()
    {
        foreach (Transform child in iconButtonParent)
        {
            Destroy(child.gameObject);
        }

        stacks = PlayerInventory.GetStacks();

        foreach (PickableStack stack in stacks)
        {
            UIQuickMenuIconButton button = Instantiate(iconButtonPrefab);
            button.SetIconSprite(stack.icon);
            button.onClick.AddListener(() => stackClicked(stack));
            button.amount.text = stack.Count > 1 ? stack.Count.ToString() : string.Empty;
            button.transform.SetParent(iconButtonParent, false);
        }

        Invoke("updateSelection", 0.0f);
    }

    void updateSelection()
    {
        selectionIndex = Mathf.Clamp(selectionIndex, 0, gridController.length - 1);
        gridController.Select(selectionIndex);
    }

    private void selectionChanged()
    {
        if (stacks.Length == 0)
        {
            itemStackLabel.text = string.Empty;
        }
        else
        {
            selectionIndex = gridController.selectedIndex;
            itemStackLabel.text = stacks[selectionIndex].label;
        }
    }

    private void stackClicked(PickableStack stack)
    {
        avatarController.ChoseInteraction(stack.firstInteractable);
    }
}

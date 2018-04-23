using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIQuickMenuPickCombatItem : UIQuickMenuPage
{

    [SerializeField]
    private Text slotNameLabel;

    [SerializeField]
    private Text combatItemLabel;

    [SerializeField]
    private UIQuickMenuIconButton iconButtonPrefab;

    [SerializeField]
    private RectTransform iconButtonParent;

    [SerializeField]
    private UIGridSelectionController gridController;

    private CombatItem[] combatItems;

    private int selectionIndex;

    public CombatItemSlot slot { get; set; }

    public int slotIndex { get; set; }



    protected override void Start()
    {
        base.Start();

        gridController.onSelectionChanged.AddListener(selectionChanged);
        gridController.inputModule = avatarController.inputModule;

        slotNameLabel.text = slot.slotName;

        PlayerInventory.collection.onItemChanged += container_onItemChanged;

        updateView();

    }

    /// <summary>
    /// Remove hook to itemChanged event
    /// </summary>
    protected override void OnDestroy()
    {
        PlayerInventory.collection.onItemChanged -= container_onItemChanged;
        base.OnDestroy();
    }

    void container_onItemChanged(Pickable pickable)
    {
        updateView();
    }

    private void updateView()
    {
        foreach (Transform child in iconButtonParent)
        {
            Destroy(child.gameObject);
        }

        combatItems = PlayerInventory.GetDistinctItemsOfType<CombatItem>();

        foreach (CombatItem stack in combatItems)
        {
            UIQuickMenuIconButton button = Instantiate(iconButtonPrefab);
            button.SetIconSprite(stack.identity.icon);
            button.onClick.AddListener(() => stackClicked(stack));
            button.amount.text = string.Empty;
            button.transform.SetParent(iconButtonParent);
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
        if (combatItems.Length == 0)
        {
            combatItemLabel.text = string.Empty;
        }
        else
        {
            selectionIndex = gridController.selectedIndex;
            combatItemLabel.text = combatItems[selectionIndex].identity.label;
        }
    }

    private void stackClicked(CombatItem combatItem)
    {
        if(avatarController.character.mannequin)
        {
            avatarController.character.mannequin.EquipCombatItem(slotIndex, combatItem);
        }

        menu.GoBack();
    }
}

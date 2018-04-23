using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIQuickMenuLoot : UIQuickMenuPage
{
    [SerializeField]
    private UIQuickMenuIconButton iconButtonPrefab;

    [SerializeField]
    private RectTransform iconButtonParent;

    [SerializeField]
    private UIGridSelectionController gridController;

    [SerializeField]
    private Text textContainerLabel;

    [SerializeField]
    private Text textStackLabel;

    [SerializeField]
    private Sprite iconTakeAll;

    private PickableStack[] stacks;

    private int selectionIndex;

    public Container container { get; set; }

    protected override void Start()
    {
        base.Start();

        gridController.onSelectionChanged.AddListener(selectionChanged);
        gridController.inputModule = avatarController.inputModule;

        container.collection.onItemChanged += container_onItemChanged;
        textContainerLabel.text = "Content of " + container.identity.label;

        updateView();

    }

    /// <summary>
    /// Remove hook to itemChanged event
    /// </summary>
    protected override void OnDestroy()
    {
        container.collection.onItemChanged -= container_onItemChanged;
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

        stacks = container.GetStacks();

        UIQuickMenuIconButton button = Instantiate(iconButtonPrefab);
        button.SetIconSprite(iconTakeAll);
        button.onClick.AddListener(() => takeAll());
        button.transform.SetParent(iconButtonParent, false);

        foreach (PickableStack stack in stacks)
        {
            UIQuickMenuIconButton stackButton = Instantiate(iconButtonPrefab);
            stackButton.SetIconSprite(stack.icon);
            stackButton.onClick.AddListener(() => stackClicked(stack));
            stackButton.amount.text = stack.Count > 1 ? stack.Count.ToString() : string.Empty;
            stackButton.transform.SetParent(iconButtonParent, false);
        }

        Invoke("updateSelection", 0.0f);
    }


    void OnEnable()
    {
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
            textStackLabel.text = string.Empty;
        }
        else
        {
            selectionIndex = gridController.selectedIndex;

            if (selectionIndex == 0)
            {
                textStackLabel.text = "Take all";
            }
            else
            {
                textStackLabel.text = "Pick up " + stacks[selectionIndex - 1].label;
            }
        }
    }

    private void takeAll()
    {
        for(int i = container.collection.Count - 1; i >= 0; i--) 
        {
            container.collection[i].PickUpToGlobalInventory();
        }

        avatarController.characterMenu.Close();
    }

    private void stackClicked(PickableStack stack)
    {
        UIQuickMenuInteractable page = Instantiate(UIFactory.uiQuickMenuInteractionCollectionPrefab);
        page.SetInteractionCollection(stack.firstInteractable);

        avatarController.characterMenu.NavigateInto(page);
    }
}

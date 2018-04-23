using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIQuickMenuStart : UIQuickMenuPage {

    // public Text label;

    private CallbackInteraction[] startMenuInteractions;

     [SerializeField]
    private UIQuickMenuEquipment quickMenuEquipmentPrefab;

     [SerializeField]
     private UIQuickMenuCombatItems quickMenuCombatItemsPrefab;

     [SerializeField]
     private UIQuickMenuAbilities quickMenuAbilitiesPrefab;


    [SerializeField]
    private UIQuickMenuInventory quickMenuInventoryPrefab;

    [SerializeField]
    private UIQuickMenuCharacterStats quickMenuStatsPrefab;

    private int selectedIndex;


    protected override void OnQuickMenuPageEnabled()
    {
        GetComponentsInChildren<UISelectable>()[selectedIndex].Select();
        base.OnQuickMenuPageEnabled();
    }

	void Start () {

       // cancelButton.SetIconSprite(avatarController.character.identity.icon);
        // GetComponent<UICircularSelectionController>().inputModule = avatarController.inputModule;
	}

    public void ShowAbilities()
    {
        selectedIndex = 1;

        UIQuickMenuCombat instance = Instantiate(UIFactory.uiQuickMenuCombatPrefab);
        instance.character = avatarController.character;
        instance.includeWalk = false;
        instance.includeDoNothing = false;

        menu.NavigateInto(instance);
    }

    public void ShowEquipment()
    {
        selectedIndex = 3;
        menu.NavigateInto(Instantiate(quickMenuEquipmentPrefab));
    }

    public void ShowCombatItems()
    {
        selectedIndex = 4;
        menu.NavigateInto(Instantiate(quickMenuCombatItemsPrefab));
    }

    public void ShowInventory()
    {
        selectedIndex = 0;
        menu.NavigateInto(Instantiate(quickMenuInventoryPrefab));
    }

    public void ShowStats()
    {
        selectedIndex = 2;
        menu.NavigateInto(Instantiate(quickMenuStatsPrefab));
    }

    public void Close()
    {
        menu.Close();
    }

    public void SetLabel(string text)
    {
         // label.text = text;
    }

    public void SetLabelToCharacterName()
    {
        if(avatarController != null && avatarController.character != null)
        {
            SetLabel(avatarController.character.identity.label);
        }
    }

   
}

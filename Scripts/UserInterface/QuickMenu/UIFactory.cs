using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFactory : MonoBehaviour {

    [SerializeField]
    private UIQuickMenu quickMenuPrefab;

    [SerializeField]
    private UIQuickMenuStart quickMenuStartPrefab;

    [SerializeField]
    private UIQuickMenuLoot quickMenuLootPrefab;

    [SerializeField]
    private UIQuickMenuInventory quickMenuInventoryPrefab;

    [SerializeField]
    private UIQuickMenuInteractable quickMenuInteractionCollectionPrefab;

    [SerializeField]
    private UIQuickMenuQuickSlots quickMenuQuickSlotsPrefab;

    [SerializeField]
    private UIQuickMenuPlacement quickMenuPlacementPrefab;

    [SerializeField]
    private UIQuickMenuCombat quickMenuCombatPrefab;

    [SerializeField]
    private UIQuickMenuCastAbility quickMenuCastAbilityPrefab;

    [SerializeField]
    private UIIconButton iconButton;

    [SerializeField]
    private UICombatItemIconButton combatItemButton;

    [SerializeField]
    private UIQuickMenuCombatItem quickMenuCombatItemPrefab;

    [SerializeField]
    private UIAbilityIconButton abilityButton;

    private static UIFactory instance;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static UIQuickMenu CreateNew()
    {
        UIQuickMenu quickMenu = Instantiate(instance.quickMenuPrefab).GetComponent<UIQuickMenu>();
        quickMenu.transform.SetParent(instance.transform, false);
        quickMenu.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
       

        return quickMenu;
    }

    public static UIQuickMenuPlacement uiQuickMenuPlacementPrefab
{
    get { return instance.quickMenuPlacementPrefab; }
}

    public static UIQuickMenuInteractable uiQuickMenuInteractionCollectionPrefab
    {
        get { return instance.quickMenuInteractionCollectionPrefab; }
    }

    public static UIQuickMenuInventory uiQuickMenuInventoryPrefab
    {
        get { return instance.quickMenuInventoryPrefab; }
    }

    public static UIQuickMenuStart uiQuickMenuStartPrefab
    {
        get { return instance.quickMenuStartPrefab; }
    }

    public static UIQuickMenuLoot uiQuickMenuLootPrefab
    {
        get { return instance.quickMenuLootPrefab; }
    }

    public static UIQuickMenuQuickSlots uiQuickMenuQuickSlotsPrefab
    {
        get { return instance.quickMenuQuickSlotsPrefab; }
    }


    public static UIQuickMenuCombat uiQuickMenuCombatPrefab
    {
        get { return instance.quickMenuCombatPrefab; }
    }

    public static UIQuickMenuCastAbility uiQuickMenuCastAbilityPrefab
    {
        get { return instance.quickMenuCastAbilityPrefab; }
    }

    public static UIAbilityIconButton uiAbilityButton
    {
        get { return instance.abilityButton; }
    }

    public static UIQuickMenuCombatItem uiQuickMenuCombatItemPrefab
    {
        get { return instance.quickMenuCombatItemPrefab; }
    }

    public static UICombatItemIconButton uiCombatItemButton
    {
        get { return instance.combatItemButton; }
    }

    public static UIIconButton uiIconButton
    {
        get { return instance.iconButton; }
    }
}

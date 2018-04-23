using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMannequin : Mannequin
{
    [SerializeField]
    private Armor helmet;

    [SerializeField]
    private Armor torso;

    [SerializeField]
    private Armor boots;

    [SerializeField]
    private CombatItem[] combatItems;

    [SerializeField]
    private string helmetSlotName = "Helmet";

    [SerializeField]
    private string torsoSlotName = "Torso";

    [SerializeField]
    private string bootsSlotName = "Boots";

    [SerializeField]
    private int combatItemSlotCount = 4;

    /// <summary>
    /// Fetches slot views from the hierarchy and assigns them to newly created slots
    /// </summary>
    protected override void Start()
    {
        ArmorSlotView[] armorSlotViews = GetComponentsInChildren<ArmorSlotView>(true);
        _armorSlots = new ArmorSlot[armorSlotViews.Length];
       
        for(int i = 0; i < _armorSlots.Length; i++)
        {
            ArmorSlotView view = armorSlotViews[i];

            switch (view.type)
            {
                case ArmorType.Helmet:
                    _armorSlots[i] = new ArmorSlot(view, helmetSlotName);
                    break;
                case ArmorType.Torso:
                    _armorSlots[i] = new ArmorSlot(view, torsoSlotName);
                    break;
                case ArmorType.Boots:
                    _armorSlots[i] = new ArmorSlot(view, bootsSlotName);
                    break;
            }
        }

        CombatItemSlotView[] combatItemSlotView = GetComponentsInChildren<CombatItemSlotView>(true);

        _combatItemSlots = new CombatItemSlot[combatItemSlotCount];

        for (int i = 0; i < _combatItemSlots.Length; i++)
        {
            _combatItemSlots[i] = new CombatItemSlot();
        }

        for (int i = 0; i < combatItemSlotView.Length; i++)
        {
             CombatItemSlotView view = combatItemSlotView[i];

             switch (view.type)
             {
                 case CombatItemSlotType.Hands:
                     _itemSlotViewHands = view;
                     break;
                 case CombatItemSlotType.Hip:
                     _itemSlotViewHip = view;
                     break;
                 case CombatItemSlotType.Back:
                     _itemSlotViewBack = view;
                     break;
             }
        }

        base.Start();

        EquipArmor(ArmorType.Helmet, helmet);
        EquipArmor(ArmorType.Torso, torso);
        EquipArmor(ArmorType.Boots, boots);

        for(int i = 0; i < combatItems.Length; i++)
        {
            EquipCombatItem(i, combatItems[i]);
        }
    }

    
}

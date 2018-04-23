using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NeverdawnArmor : MonoBehaviour {

    [HideInInspector]
    public Character character;

    public ArmorSlot[] slots;

    private Dictionary<ArmorType, ArmorSlot> armorSlots;

    void Awake()
    {
        armorSlots = new Dictionary<ArmorType, ArmorSlot>();

        foreach(ArmorSlot slot in slots)
        {
            armorSlots.Add(slot.type, slot);
        }
    }

    public void UnEquip(ArmorType type)
    {
        ArmorSlot slot = armorSlots[type];

        if (slot != null && slot.isOccupied)
        {
            slot.Set(null);
        }
    }

    public void Equip(Armor armor)
    {
        ArmorSlot slot = armorSlots[armor.type];

        if (slot != null && armor != null)
        {
            slot.Set(armor);
        }
    }

    public Armor[] GetEquipment()
    {
        return GetComponentsInChildren<Armor>(true);
    }

    public Armor torso
    {
        get
        {
            return armorSlots[ArmorType.Torso].Get();
        }
    }

    public Armor helmet
    {
        get
        {
            return armorSlots[ArmorType.Helmet].Get();
        }
    }

    public Armor boots
    {
        get
        {
            return armorSlots[ArmorType.Boots].Get();
        }
    }

    internal int GetSkillBonus(SkillType characterSkillType)
    {
        int skillBonus = 0;

        foreach (ArmorSlot slot in slots)
        {
            if(slot.isOccupied)
            {
                skillBonus += slot.Get().GetSkillBonus(characterSkillType);
            }
        }

        return skillBonus;
    }

    internal int GetAttributeBonus(AttributeType characterAttributeType)
    {
        int attributeBonus = 0;

        foreach (ArmorSlot slot in slots)
        {
            if (slot.isOccupied)
            {
                attributeBonus += slot.Get().GetAttributeBonus(characterAttributeType);
            }
        }

        return attributeBonus;
    }
}

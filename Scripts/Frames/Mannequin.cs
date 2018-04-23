using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mannequin : FrameComponent {

    protected CombatItemSlotView _itemSlotViewHands;

    protected ArmorSlot[] _armorSlots;

    protected CombatItemSlot[] _combatItemSlots;

    protected CombatItemSlotView _itemSlotViewHip;

    protected CombatItemSlotView _itemSlotViewBack;

    public CombatItemSlot[] combatItemSlots
    {
        get { return _combatItemSlots; }
    }

    public ArmorSlot[] armorSlots
    {
        get { return _armorSlots; }
    }

    public void ShowItemInHandsView(CombatItem combatItem)
    {
        if (_itemSlotViewHands != null)
        {
            _itemSlotViewHands.SetCombatItem(combatItem);
        }
    }

    public int armorValue
    {
        get { return _armorSlots.Where(s => s.Get()).Select(s => s.Get()).Sum(a => a.armorValue); }
    }

    public float armorMitigation
    {
        get { return 1.0f / (GameSettings.armorMitigationScale * armorValue + 1); }
    }

    protected virtual void Start()
    {
        foreach(ArmorSlot armorSlot in _armorSlots)
        {
            armorSlot.transform = transform;
        }

        foreach(CombatItemSlot itemSlot in _combatItemSlots)
        {
            itemSlot.transform = transform;
        }
    }

    /// <summary>
    /// Reads the equipment data from a message reader
    /// </summary>
    /// <param name="reader"></param>
    protected override void readData(IMessageReader reader)
    {
        int numArmorSlots = reader.ReadInt();

        for (int i = 0; i < numArmorSlots; i++)
        {
            ArmorType type = (ArmorType)reader.ReadByte();

            string frameId = reader.ReadString();
            Frame frame = Frame.FindFrameById(frameId);

            if (frame != null)
            {
                StartCoroutine(equipAmorDeferred(frame));
            }
        }

        int numCombatItemSlots = reader.ReadInt();

        for (int i = 0; i < numCombatItemSlots; i++)
        {
            int index = reader.ReadInt();
            Frame frame = Frame.FindFrameById(reader.ReadString());

            if (frame != null)
            {
                StartCoroutine(equipCombatItemDeferred(index, frame));
            }
        } 
    }

    /// <summary>
    /// writes the current equipment data to a message writer
    /// </summary>
    /// <param name="writer"></param>
    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteInt(_armorSlots.Length);

        foreach (ArmorSlot slot in _armorSlots)
        {
            writer.WriteByte((byte)slot.type);
            writer.WriteString(slot.isOccupied ? slot.pickable.frame.id : string.Empty);
        }

        int slotIndex = 0;

        writer.WriteInt(_combatItemSlots.Length);

        foreach (CombatItemSlot slot in _combatItemSlots)
        {
            writer.WriteInt(slotIndex++);
            writer.WriteString(slot.isOccupied ? slot.pickable.frame.id : string.Empty);
        }
    }

    /// <summary>
    /// equip an armor piece after the next frame
    /// </summary>
    /// <param name="pickableIds"></param>
    /// <returns></returns>
    private IEnumerator equipAmorDeferred(Frame frame)
    {
        yield return null;

        Armor armor = frame.GetComponent<Armor>();

        if (armor != null)
        {
            EquipArmor(armor.type, armor);
        }
    }

    public void EquipArmor(ArmorType type, Armor armor)
    {
        ArmorSlot slot = getArmorSlotByType(type);

        if (slot != null)
        {
            slot.Set(armor);
        }
    }

    /// <summary>
    /// equip a combat item after the next frame
    /// </summary>
    /// <param name="pickableIds"></param>
    /// <returns></returns>
    private IEnumerator equipCombatItemDeferred(int index, Frame frame)
    {
        yield return null;

        CombatItem combatItem = frame.GetComponent<CombatItem>();

        if (combatItem != null)
        {
            EquipCombatItem(index, combatItem);
        }
    }

    /// <summary>
    /// equip a combat item
    /// </summary>
    /// <param name="index"></param>
    /// <param name="combatItem"></param>
    public void EquipCombatItem(int index, CombatItem combatItem)
    {
        CombatItemSlot slot = getCombatItemSlotByIndex(index);

        if (slot != null)
        {
            slot.Set(combatItem);
        }
    }

    /// <summary>
    /// Returns a combat item slot or null
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private CombatItemSlot getCombatItemSlotByIndex(int p)
    {
        if (p >= 0 && p < _combatItemSlots.Length)
        {
            return _combatItemSlots[p];
        }

        return null;
    }

    /// <summary>
    /// Returns an armor slot or null
    /// </summary>
    /// <param name="armorType"></param>
    /// <returns></returns>
    private ArmorSlot getArmorSlotByType(ArmorType armorType)
    {
        foreach (ArmorSlot slot in _armorSlots)
        {
            if (slot.type == armorType)
            {
                return slot;
            }
        }

        return null;
    }

    internal CombatItem GetFirstEquippedCombatItem()
    {
        foreach (CombatItemSlot slot in combatItemSlots)
        {
            if (slot.isOccupied)
            {
                return slot.Get();
            }
        }

        return null;
    }

    internal CombatItem[] GetEquippedCombatItems() 
    {
        List<CombatItem> result = new List<CombatItem>();

        foreach(CombatItemSlot slot in combatItemSlots)
        {
            if(slot.isOccupied)
            {
                result.Add(slot.Get());
            }
        }

        return result.ToArray();
    }

    internal int GetAttributeBonus(AttributeType attributeType)
    {
        return 0;
    }

    internal int GetSkillBonus(SkillType skillType)
    {
        return 0;
    }

    internal void Equip(Equipable equipable)
    {
        if (equipable != null)
        {
            if (equipable is Armor)
            {
                Armor armor = equipable as Armor;
                EquipArmor(armor.type, armor);
                return;
            }

            if (equipable is CombatItem)
            {
                int k = 0;

                foreach (CombatItemSlot slot in combatItemSlots)
                {
                    if (!slot.isOccupied)
                    {
                        EquipCombatItem(k, equipable as CombatItem);
                        return;
                    }

                    k++;
                }
            }
        }
    }

    internal bool CanEquip(Equipable equipable)
    {
        if(equipable is Armor)
        {
            return true;
        }

        if(equipable is CombatItem)
        {
            foreach(CombatItemSlot slot in combatItemSlots)
            {
                if(!slot.isOccupied)
                {
                    return true;
                }
            }
        }

        return false;
    }

}

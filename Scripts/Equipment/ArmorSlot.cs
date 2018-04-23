using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArmorSlot : PickableSlot<Armor>
{
    public ArmorType type { get; private set; }

    public ArmorSlotView view { get; private set; }

    public ArmorSlot(ArmorSlotView view, string name)
    {
        this.type = view.type;
        this.view = view;
        this.slotName = name;
    }

    protected override void OnComponentAdded(Armor component)
    {
        component.isEquipped = true;
        view.SetArmor(component);
    }

    protected override void OnComponentRemoved(Armor component)
    {
        component.isEquipped = false;
        view.SetArmor(null);
    }

}

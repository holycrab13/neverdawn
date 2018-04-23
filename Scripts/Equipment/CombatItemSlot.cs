using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatItemSlot : PickableSlot<CombatItem> {

    protected override void OnComponentAdded(CombatItem component)
    {
        component.isEquipped = true;
    }

    protected override void OnComponentRemoved(CombatItem component)
    {
        component.isEquipped = false;
    }

    internal T GetEquipped<T>() where T : FrameComponent
    {
        T component = pickable.GetComponent<T>();
        return component;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NeverdawnCombatItems : MonoBehaviour {

    public CombatItemSlotView[] slotViews;

    public CombatItemSlot[] slots;

    [SerializeField]
    private CombatItemSlotView handsView;

    [SerializeField]
    private CombatItem _unarmedWeapon;

    private Dictionary<CombatItemSlotType, CombatItemSlotView> viewsByType;

    private Dictionary<CombatItem, CombatItemSlotView> itemViews;

    void Awake()
    {
        itemViews = new Dictionary<CombatItem, CombatItemSlotView>();
        viewsByType = new Dictionary<CombatItemSlotType, CombatItemSlotView>();

        foreach (CombatItemSlotView view in slotViews)
        {
            viewsByType.Add(view.type, view);
        }
    }

    internal void Equip(CombatItem combatItem, CombatItemSlot slot)
    {
        slot.Set(combatItem);
    }

    internal void UpdateView()
    {
        foreach (CombatItemSlotView view in slotViews)
            view.SetCombatItem(null);

        itemViews.Clear();

        foreach (CombatItem item in GetAll())
        {
            CombatItemSlotView view = findEmptyView(item);

            if (view != null)
            {
                view.SetCombatItem(item);
                itemViews.Add(item, view);
            }
        }
    }

    private CombatItemSlotView findEmptyView(CombatItem item)
    {
        if(item.preferredSlot == CombatItemSlotType.Back || item.preferredSlot == CombatItemSlotType.Hip)
        {
     
            if (viewsByType.ContainsKey(item.preferredSlot) && viewsByType[item.preferredSlot].isEmpty)
                return viewsByType[item.preferredSlot];
            
        }

        return null;
    }


    public CombatItem UnEquip(CombatItemSlot slot)
    {
        if (slot.isOccupied)
        {
            CombatItem combatItem = slot.Get();

            slot.Set(null);

            return combatItem;
        }

        return null;
    }
    
    private CombatItemSlot FindEmptySlot()
    {
        foreach(CombatItemSlot slot in slots)
        {
            if(!slot.isOccupied)
            {
                return slot;
            }
        }

        return null;
    }

    internal CombatItem GetFirst()
    {
        foreach (CombatItemSlot slot in slots)
        {
            if (slot.isOccupied)
            {
                return slot.Get();
            }
        }

        return null;
    }

    internal List<CombatItem> GetAll()
    {
        List<CombatItem> result = new List<CombatItem>();

        foreach (CombatItemSlot slot in slots)
        {
            if (slot.isOccupied)
            {
                result.Add(slot.Get());
            }
        }

        return result;
    }

    public int Length
    {
        get { return slots.Length; }
    }


    internal void ShowActiveCombatItem(CombatItem activeCombatItem)
    {
        if (activeCombatItem != null)
        {
            if(itemViews.ContainsKey(activeCombatItem))
                itemViews[activeCombatItem].SetCombatItem(null);

            if (handsView != null)
                handsView.SetCombatItem(activeCombatItem);
        }
        else
        {
            foreach (KeyValuePair<CombatItem, CombatItemSlotView> pair in itemViews)
            {
                pair.Value.SetCombatItem(pair.Key);   
            }

            if(handsView != null)
                handsView.SetCombatItem(null);
        }
    }


    internal void Draw(CombatItem item)
    {
        ShowActiveCombatItem(item);
    }

    internal void SetActiveCombatItem(int p)
    {
        if(slots[p].isOccupied)
        {
            ShowActiveCombatItem(slots[p].Get());
        }
    }

    public CombatItem unarmedWeapon
    {
        get { return _unarmedWeapon; }
    }

    internal T[] GetEquipped<T>() where T : FrameComponent
    {
        List<T> result = new List<T>();

        foreach (CombatItemSlot slot in slots)
        {
            if (slot.isOccupied)
            {
                T component = slot.GetEquipped<T>();

                if(component != null)
                {
                    result.Add(component);
                }
            }
        }

        return result.ToArray();
    }
}

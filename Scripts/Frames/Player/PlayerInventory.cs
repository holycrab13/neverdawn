using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerInventory : FrameComponent
{
    public static PlayerInventory instance;

    [SerializeField]
    private PickableCollection _collection;

    [SerializeField]
    private int _gold;

    void Awake()
    {
        _collection = new PickableCollection(transform);
    }

    /// <summary>
    /// Add gold to the purse
    /// </summary>
    /// <param name="amount"></param>
    public static void AddGold(int amount)
    {
        instance._gold += amount;
    }

    /// <summary>
    /// Removes gold from the purse
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static int RemoveGold(int amount)
    {
        int goldAmount = Mathf.Min(amount, instance._gold);

        instance._gold -= goldAmount;
        return goldAmount;
    }

    protected override void readData(IMessageReader reader)
    {
        string[] pickableIds = reader.ReadStringArray();
        _gold = reader.ReadInt();

        foreach (string pickableId in pickableIds)
        {
            Frame frame = Frame.FindFrameById(pickableId);

            if (frame != null)
            {
                Pickable pickable = frame.GetComponent<Pickable>();

                if (pickable != null)
                {
                    _collection.AddPickable(pickable);
                }
            }
        }
    }

    protected override void writeData(IMessageWriter writer)
    {
        List<string> pickableIds = new List<string>();


        foreach (Pickable pickable in _collection)
        {
            pickableIds.Add(pickable.frame.id);
        }


        writer.WriteStringArray(pickableIds.ToArray());
        writer.WriteInt(_gold);
    }

    public static PickableCollection collection
    {
        get { return instance._collection; }
    }

    internal void Initialize(SerializableGame data)
    {
        instance = this;
    }

    internal static void PutItem(Pickable item)
    {
        if (instance != null)
        {
            instance._collection.AddPickable(item);
        }
    }

    public static Pickable PullItem(Pickable item)
    {
        if (instance._collection.RemovePickable(item))
        {
            item.transform.SetParent(null, false);
            item.solid.Show();

            return item;
        }

        return null;
    }

    internal static PickableStack[] GetStacks()
    {
        return PickableStack.CreateStacks(instance._collection);
    }


    public static void AddItemChangedListener(Action<Pickable> listener)
    {
        instance._collection.onItemChanged += new PickableCollection.PickableEvent(listener);
    }

    public static void RemoveItemChangedListener(Action<Pickable> listener)
    {
        instance._collection.onItemChanged -= new PickableCollection.PickableEvent(listener);
    }


    internal static T[] GetDistinctItemsOfType<T>(Func<T, bool> predicate = null)
    {
        PickableStack[] stacks = PickableStack.CreateStacks(instance._collection);
        List<T> result = new List<T>();


        foreach(PickableStack stack in stacks)
        {
            T item = stack.firstInteractable.GetComponent<T>();

            if(item != null)
            {
                if(predicate == null || predicate(item))
                    result.Add(item);
            }
        }

        return result.ToArray();
    }

    internal static void Clear()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSlot<T> : PickableCollection where T : FrameComponent
{
    public string slotName;

    [HideInInspector]
    public Pickable pickable;

    public bool isOccupied
    {
        get { return Count != 0; }
    }

    protected override void OnPickableAdded(Pickable pickable)
    {
        OnComponentAdded(pickable.GetComponent<T>());
    }

    protected override void OnPickableRemoved(Pickable pickable)
    {
        OnComponentRemoved(pickable.GetComponent<T>());
    }

    protected virtual void OnComponentAdded(T component)
    {

    }

    protected virtual void OnComponentRemoved(T component)
    {

    }

    public T Get()
    {
        if (pickable == null)
            return null;

        return pickable.GetComponent<T>();
    }

    public void Set(T component)
    {
        if (component == null)
        {
            if (pickable != null)
            {
                PlayerInventory.PutItem(pickable);
            }

            this.pickable = null;
        }
        else
        {
            Pickable pickableToSet = component.GetComponent<Pickable>();

            if (pickableToSet != null)
            {
                if (pickable != null)
                {
                    PlayerInventory.PutItem(pickable);
                }

                this.pickable = pickableToSet;

                if (pickableToSet != null)
                {
                    AddPickable(pickableToSet);
                }
            }
        }
    }
}

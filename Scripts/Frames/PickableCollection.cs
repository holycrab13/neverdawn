using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;



[Serializable]
public class PickableCollection : IEnumerable<Pickable>
{
    public delegate void PickableEvent(Pickable pickable);

    public event PickableEvent onItemAdded;

    public event PickableEvent onItemRemoved;

    public event PickableEvent onItemChanged;

    [SerializeField]
    protected List<Pickable> pickables = new List<Pickable>();

    public Transform transform { get; set; }

    public PickableCollection(Transform transform = null)
    {
        this.transform = transform;
    }

    public Pickable this[int index]
    {
        get { return pickables[index]; }
    }

    protected virtual void OnPickableAdded(Pickable pickable)
    {

    }

    protected virtual void OnPickableRemoved(Pickable pickable)
    {

    }

    public void AddPickable(Pickable pickable)
    {
        if (pickable.collection != this)
        {
            if (pickable.collection != null)
                pickable.collection.RemovePickable(pickable);

            pickable.collection = this;
            pickable.solid.Hide();
            pickable.transform.SetParent(transform);
            pickable.transform.localPosition = Vector3.zero;
            pickable.transform.localEulerAngles = Vector3.zero;

            pickables.Add(pickable);

            OnPickableAdded(pickable);

            if (onItemChanged != null)
            {
                onItemChanged.Invoke(pickable);
            }

            if (onItemAdded != null)
            {
                onItemAdded.Invoke(pickable);
            }

        }
    }

    public bool RemovePickable(Pickable pickable)
    {
        if (pickable != null && pickable.collection == this)
        {

            bool sucess = pickables.Remove(pickable);

            if(sucess)
            {
                pickable.collection = null;
                pickable.transform.SetParent(null);

                OnPickableRemoved(pickable);

                if (onItemChanged != null)
                {
                    onItemChanged.Invoke(pickable);
                }

                if (onItemRemoved != null)
                {
                    onItemRemoved.Invoke(pickable);
                }

                return true;
            }

            return false;
        }

        return false;
    }

    public IEnumerator<Pickable> GetEnumerator()
    {
        return pickables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return pickables.GetEnumerator();
    }
    
    public int Count
    {
        get { return pickables.Count; }
    }
}


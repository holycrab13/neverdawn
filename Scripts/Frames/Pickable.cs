using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


[AddComponentMenu("FrameComponent/Pickable")]
[RequireComponent(typeof(Identity))]
[RequireComponent(typeof(Solid))]
public class Pickable : FrameComponent
{
    public string stackId;

    private Solid _solid;

    public PickableCollection collection { get; set; }

    public Solid solid
    {
        get
        {
            if (_solid == null)
            {
                _solid = GetComponent<Solid>();
            }

            return _solid;
        }
    }

    /// <summary>
    /// Pick a pickable to the inventory
    /// </summary>
    public void PickUpToGlobalInventory()
    {
        if (GetComponent<OnPickTrigger>())
        {
            GetComponent<OnPickTrigger>().Trigger();
        }

        PlayerInventory.PutItem(this);
    }

    /// <summary>
    /// Drop an item at a given transform
    /// </summary>
    /// <param name="character"></param>
    public void DropAtLocation(Transform dropLocation)
    {
        if(collection != null)
        {
            collection.RemovePickable(this);

            solid.Show();
            transform.position = dropLocation.position;
            transform.rotation = dropLocation.rotation;
        }
    }

    public void RemoveFromCollection()
    {
        if (collection != null)
        {
            collection.RemovePickable(this);
        }
    }

    public bool isPickable
    {
        get
        {
            return collection == null || collection != PlayerInventory.collection;
        }
    }

    void OnDestroy()
    {
        if (collection != null)
        {
            collection.RemovePickable(this);
        }
    }
}

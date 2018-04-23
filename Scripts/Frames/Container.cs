using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Frame))]
public class Container : FrameComponent
{
    
    private Identity _identity;

    private PickableCollection _collection;

    [SerializeField]
    private List<Pickable> content;

    public PickableCollection collection
    {
        get { return _collection; }
    }

    void Awake()
    {
        _collection = new PickableCollection(transform);

        foreach (Pickable pickable in content)
        {
            _collection.AddPickable(pickable);
        }
    }


    protected override void readData(IMessageReader reader)
    {
        string[] pickableIds = reader.ReadStringArray();

        foreach (string pickableId in pickableIds)
        {
            Frame frame = Frame.FindFrameById(pickableId);

            if(frame != null)
            {
                Pickable equipable = frame.GetComponent<Pickable>();

                if(equipable != null)
                {
                    _collection.AddPickable(equipable);
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
    }

    internal PickableStack[] GetStacks()
    {
        return PickableStack.CreateStacks(_collection);
    }

    private List<Pickable> getPickablesInChildren()
    {
        List<Pickable> items = new List<Pickable>();

        foreach (Transform t in transform)
        {
            Pickable pickable = t.GetComponent<Pickable>();

            if (pickable != null)
            {
                items.Add(pickable);
            }
        }

        return items;
    }

    public Identity identity
    {
        get
        {
            if (_identity == null)
            {
                _identity = GetComponent<Identity>();
            }

            return _identity;
        }
    }

    public bool isEmpty
    {
        get { return collection.Count == 0; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PickableStack : List<Pickable>
{
    public Interactable firstInteractable;

    public PickableStack(Pickable pickable)
    {
        firstInteractable = pickable.GetComponent<Interactable>();
        Add(pickable);
    }

    public static PickableStack[] CreateStacks(IEnumerable<Pickable> pickables)
    {
        Dictionary<string, PickableStack> dict = new Dictionary<string, PickableStack>();

        foreach (Pickable pickable in pickables)
        {
            if (pickable.stackId == null || pickable.stackId.Equals(string.Empty))
            {
                dict.Add(Guid.NewGuid().ToString(), new PickableStack(pickable));
            }
            else
            {
                if (!dict.ContainsKey(pickable.stackId))
                {
                    dict.Add(pickable.stackId, new PickableStack(pickable));
                }
                else
                {
                    dict[pickable.stackId].Add(pickable);
                }
            }
        }

        return dict.Values.ToArray();

    }

    public Sprite icon
    {
        get { return firstInteractable.identity.icon; }
    }

    public string label
    {
        get { return firstInteractable.identity.label; }
    }

    public string description
    {
        get { return firstInteractable.identity.description; }
    }

    internal void PickUpToGlobalInventory()
    {
        if(Count > 0)
        {
            this[0].PickUpToGlobalInventory();
        }
    }
}


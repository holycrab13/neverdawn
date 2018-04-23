using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interaction", menuName = "Neverdawn/Interaction/Loot Purse", order = 1)]
public class LootPurseInteraction : ComponentInteraction
{
    private Pickable pickable;

    private Purse purse;

    public override void Interact(NeverdawnCharacterController controller)
    {
        if(pickable.isPickable)
        {
            pickable.RemoveFromCollection();
            PlayerInventory.AddGold(purse.gold);
        }


        pickable.frame.destroyed = true;
        Destroy(pickable.gameObject);
    }

    public override bool IsAvailable(Character character)
    {
        return true;
    }

    public override bool Initialize(GameObject gameObject)
    {
        pickable = gameObject.GetComponent<Pickable>();
        purse = gameObject.GetComponent<Purse>();

        return pickable != null && purse != null;
    }
}
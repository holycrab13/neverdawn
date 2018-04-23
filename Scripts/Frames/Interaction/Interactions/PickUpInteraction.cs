using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interaction", menuName = "Neverdawn/Interaction/PickUp", order = 1)]
public class PickUpInteraction : ComponentInteraction
{
    private Pickable pickable;

    public Sprite altIcon;

    public string altLabel;

    public int altPriority;

    public override void Interact(NeverdawnCharacterController controller)
    {
        if(pickable.isPickable)
        {
        
            pickable.PickUpToGlobalInventory();
        }
        else
        {
            pickable.DropAtLocation(controller.character.transform);
        }  
    }

    public override bool IsAvailable(Character character)
    {
        return true;
    }

    public override bool Initialize(GameObject gameObject)
    {
        pickable = gameObject.GetComponent<Pickable>();

        return pickable != null;
    }

    public override Sprite GetIcon()
    {
        return pickable.isPickable ? icon : altIcon;
    }

    public override string GetLabel()
    {
        return pickable.isPickable ? label : altLabel;
    }

    public override int GetPriority()
    {
        return pickable.isPickable ? priority : altPriority;
    }
}
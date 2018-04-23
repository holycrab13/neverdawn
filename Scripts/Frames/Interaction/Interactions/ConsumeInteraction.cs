using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interaction", menuName = "Neverdawn/Interaction/Consume", order = 1)]
public class ConsumeInteraction : ComponentInteraction
{
    private Consumable consumable;

    public override void Interact(NeverdawnCharacterController controller)
    {
        controller.character.Consume(consumable);
    }

    public override bool IsAvailable(Character character)
    {
        return true;
    }

    public override bool Initialize(GameObject gameObject)
    {
        consumable = gameObject.GetComponent<Consumable>();

        return consumable != null;
    }

   
}
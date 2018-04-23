using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interaction", menuName = "Neverdawn/Interaction/Fight", order = 1)]
public class FightInteraction : ComponentInteraction
{
    private Fighter fighter;

    public override void Interact(NeverdawnCharacterController controller)
    {
        GameController.instance.StartCombat(fighter.GetComponent<Character>());
    }

    public override bool IsAvailable(Character character)
    {
        return true;
    }

    public override bool Initialize(GameObject gameObject)
    {
        fighter = gameObject.GetComponent<Fighter>();

        return fighter != null;
    }

   
}
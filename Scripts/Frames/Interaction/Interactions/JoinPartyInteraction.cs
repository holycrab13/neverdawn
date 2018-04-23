using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interaction", menuName = "Neverdawn/Interaction/Join Party", order = 1)]
public class JoinPartyInteraction : ComponentInteraction
{
    private Character character;

    public override void Interact(NeverdawnCharacterController controller)
    {
        //if (PlayerParty.HasRoom())
        //{
        //    PlayerParty.AddCharacter(character);
        //}
    }

    public override bool Initialize(GameObject gameObject)
    {
        character = gameObject.GetComponent<Character>();

        return character != null;
    }
}
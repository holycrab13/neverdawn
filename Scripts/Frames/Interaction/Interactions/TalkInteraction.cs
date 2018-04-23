using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interaction", menuName = "Neverdawn/Interaction/Talk", order = 1)]
public class TalkInteraction : ComponentInteraction
{
    private Talker talker;


    public override void Interact(NeverdawnCharacterController controller)
    {
        if(controller is AvatarController)
            talker.Talk(controller as AvatarController);
    }

    public override bool IsAvailable(Character character)
    {
        return true;
    }

    public override bool Initialize(GameObject gameObject)
    {
        talker = gameObject.GetComponent<Talker>();

        return talker != null;
    }

   
}
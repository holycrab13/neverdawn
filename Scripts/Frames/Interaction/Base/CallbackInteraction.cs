using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Callback", menuName = "Neverdawn/Interaction/Callback", order = 1)]
public class CallbackInteraction : QuickMenuInteraction
{
    public Action<NeverdawnCharacterController> callback;

    public override void Interact(NeverdawnCharacterController controller)
    {
        callback(controller);

        base.Interact(controller);
    }
}
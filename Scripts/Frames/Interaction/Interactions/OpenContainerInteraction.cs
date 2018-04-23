using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interaction", menuName = "Neverdawn/Interaction/Open", order = 1)]
public class OpenContainerInteraction : ComponentInteraction
{
    private Container container;

    public override void Interact(NeverdawnCharacterController controller)
    {
        controller.Loot(container);
    }

    public override bool Initialize(GameObject gameObject)
    {
        container = gameObject.GetComponent<Container>();

        return container != null;
    }
}
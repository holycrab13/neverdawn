using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableStackInteraction : QuickMenuInteraction
{
    private PickableStack stack;

    public PickableStackInteraction(PickableStack stack)
    {
        this.stack = stack;
        this.icon = stack.icon;
        this.label = stack.label;
        this.description = stack.description;
    }

    public int amount
    {
        get { return stack.Count; }
    }

    public override void Interact(NeverdawnCharacterController controller)
    {
        controller.ChoseInteraction(stack.firstInteractable);
    }
}
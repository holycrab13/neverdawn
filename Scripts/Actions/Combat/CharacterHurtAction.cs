using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class CharacterHurtAction : CharacterActionBase, IExitStateListener
{

    protected override void OnCharacterActionStart(Character character)
    {


        Animator animator = character.animator;


        ExitAlertBehaviour.AlertExitState(animator, "Hurt", this);

        animator.SetTrigger("Hurt");

    }

    public override void ActionUpdate(float timekey)
    {
        if (IsCancelled)
            Done();
    }

    public void OnExitState()
    {
        Done();
    }
}




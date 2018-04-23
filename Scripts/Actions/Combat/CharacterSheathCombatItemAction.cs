using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class CharacterSheathCombatItemAction : CharacterActionBase, IExitStateListener, IEnterStateListener
{
    private float timer;

    private float seatheTime;
    private Animator animator;

    private float fadeDuration = 0.3f;

    public CharacterSheathCombatItemAction()
    {
       
    }

    protected override void OnCharacterActionStart(Character actor)
    {

        if(actor.activeCombatItem == null)
        {
            Done();
            return;
        }

        animator = actor.animator;

        if (animator)
        {
            if (actor.activeCombatItem.drawableLeftHand == null && actor.activeCombatItem.drawableRightHand == null)
            {
               // ExitAlertBehaviour.AlertExitState(animator, "CombatStance", this);
                animator.SetTrigger("ToGrounded");
            }
            else
            {
                //ExitAlertBehaviour.AlertExitState(animator, "SheatheWeapon", this);
                animator.SetTrigger("SheatheWeapon");
            }
        }


        Done();

        actor.activeCombatItem = null;
        actor.activeAbility = null;

    }

    public override void ActionUpdate(float timekey)
    {
     
    }

    public void OnExitState()
    {
    }

    public void OnEnterState()
    {
        character.Invoke("done", 0.3f);
    }

    private void done()
    {
        Done();
    }
}




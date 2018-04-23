using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class CharacterDieAction : CharacterActionBase
{

    protected override void OnCharacterActionStart(Character character)
    {
        Animator animator = character.animator;

        animator.SetTrigger("Die");

        Done();

    }

    public override void ActionUpdate(float timekey)
    {
        
    }

}




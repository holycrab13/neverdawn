using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterCastBuffOnTargetAction : CharacterActionBase, IExitStateListener
{
    private Frame target;
    private float timer;
    private bool effectsApplied;
    private bool targetSelf;
    private float applyDelay;
    private GameObject applyEffect;
    private IEnumerable<BuffBase> buffs;

    public CharacterCastBuffOnTargetAction(IEnumerable<BuffBase> buffs, GameObject applyEffect, float applyDelay, 
        Frame target, bool targetSelf)
    {
        this.buffs = buffs;
        this.applyEffect = applyEffect;
        this.applyDelay = applyDelay;

        this.target = target;
        this.targetSelf = targetSelf;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        Animator animator = actor.GetComponent<Animator>();

        ExitAlertBehaviour.AlertExitState(animator, "Attack", this);

        animator.SetTrigger("Attack");


        timer = 0.0f;
    }

    public override void ActionUpdate(float timekey)
    {
        timer += timekey;

        if(timer > applyDelay && !effectsApplied)
        {
            applyEffects();
            effectsApplied = true;
        }
    }

    private void applyEffects()
    {
        if (applyEffect != null)
        {
            GameObject instance = GameObject.Instantiate(applyEffect);
            instance.transform.position = targetSelf ? character.position : target.position;
        }

        target.AddBuffs(buffs);
    }

    public void OnExitState()
    {
        if (!effectsApplied)
        {
            applyEffects();
        }

        Done();
    }
}


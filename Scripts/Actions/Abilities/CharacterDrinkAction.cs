using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterConsumeAction : CharacterActionBase, IExitStateListener
{
    private Consumable consumable;
    private float consumeDelay;
    private Animator animator;
    private bool effectsApplied;
    private float timer;

    public CharacterConsumeAction(Consumable consumable, float consumeDelay)
    {
        this.consumeDelay = consumeDelay;
        this.consumable = consumable;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        animator = actor.animator;

        ExitAlertBehaviour.AlertExitState(animator, "Attack", this);

        animator.SetTrigger("Attack");

        timer = 0.0f;
        effectsApplied = false;
    }

    public override void ActionUpdate(float timekey)
    {
        timer += timekey;

        if (timer > consumeDelay && !effectsApplied)
        {
            // apply damage and buffs;
            applyEffects();
            effectsApplied = true;
        }
    }

    private void applyEffects()
    {
        character.Consume(consumable);
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


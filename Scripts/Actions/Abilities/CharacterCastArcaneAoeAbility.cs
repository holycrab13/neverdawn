using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterCastArcaneAoeAbility : CharacterActionBase, IExitStateListener
{
    private ArcaneDamageAoeAbility ability;
    private float timer;
    private bool abilityFired;
    private bool effectFired;


    public CharacterCastArcaneAoeAbility(ArcaneDamageAoeAbility ability)
    {
        this.ability = ability;
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

        if (timer > ability.effectDelay && !effectFired)
        {
            GameObject instance = GameObject.Instantiate(ability.aoeEffect);
            instance.transform.position = character.position;
            effectFired = true;
        }

        if(timer > ability.damageDelay && !abilityFired)
        {

            Character[] characters = GameController.GetEnemiesInRange(character.position, ability.aoeRange);

            foreach (Character charInRange in characters)
            {
                charInRange.GetComponent<Destructible>().Damage((int)(ability.aoeDamage * ability.arcaneObject.arcanePower));
            }

            abilityFired = true;
        }
    }

    public void OnExitState()
    {
        Done();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterMeleeAttackAbilityAction : CharacterActionBase, IExitStateListener
{
    private Frame target;
    private Weapon weapon;
    private Animator animator;
    private float damageDelay;
    private float timer;
    private bool effectsApplied;
    private float damageModifier;
    private Destructible destructible;
    private BuffBase[] buffs;
    private GameObject effect;
    private bool targetSelf;
  
    public CharacterMeleeAttackAbilityAction(MeleeAttackAbility ability)
    {
        this.weapon = ability.weapon;
        this.target = ability.target;
        this.targetSelf = ability.targetSelf;
        this.damageDelay = ability.damageDelay;
        this.damageModifier = ability.damageModifier;
        this.destructible = target.GetComponent<Destructible>();
        this.buffs = ability.onHitBuffs;
        this.effect = ability.onHitEffect;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        animator = actor.animator;

        ExitAlertBehaviour.AlertExitState(animator, "Attack", this);
      
        animator.SetTrigger("Attack");

        timer = 0.0f;
    }

    public override void ActionUpdate(float timekey)
    {
        timer += timekey;

        if(timer > damageDelay && !effectsApplied)
        {
            // apply damage and buffs;
            applyEffects();
            effectsApplied = true;
        }
    }

    private void applyEffects()
    {
        if (effect != null)
        {
            GameObject instance = GameObject.Instantiate(effect);
            instance.transform.position = targetSelf ? character.position : target.position;
        }

        destructible.Damage(CombatUtils.GetDamageToDestructible(destructible, weapon.baseDamage * damageModifier));
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


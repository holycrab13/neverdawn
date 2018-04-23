using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterThrowAttackAbillityAction : CharacterActionBase
{
    private Vector3 velocity;
    private Animator animator;
    private float duration;
    private float spawnDelay;
    private float timer;
    private bool projectileSpawned;
    private Projectile projectile;
    private Projectile projectileInstance;
    private float damageModifer;
    private float baseDamage;
    private float maxRange;
    private Vector3 projectileSpawn;
    private float timeOfFlight;
    private Frame target;

    public CharacterThrowAttackAbillityAction(ThrowAbility ability)
    {
        this.timeOfFlight = ability.timeOfFlight;
        this.target = ability.target;
        this.velocity = ability.targetVelocity;
        this.spawnDelay = ability.spawnDelay;
        this.projectile = ability.projectile;
        this.projectileSpawn = ability.throwable.throwOffset;
        this.baseDamage = ability.throwable.baseDamage;
        this.damageModifer = ability.throwable.damageModifier;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        animator = actor.GetComponentInChildren<Animator>();
        animator.SetTrigger("Attack");

        timer = 0.0f;
    }

    public override void ActionUpdate(float timekey)
    {
        timer += timekey;

        if (timer > spawnDelay && !projectileSpawned)
        {
            // apply damage
            Vector3 spawnPoint = character.transform.position + character.transform.TransformDirection(projectileSpawn);

            projectileInstance = GameObject.Instantiate(projectile);
            projectileInstance.transform.position = spawnPoint;
            projectileInstance.character = character;
            projectileInstance.velocity = velocity;
            projectileInstance.damage = baseDamage * damageModifer + 0.2f * character.GetAttributeLevel(AttributeType.Strength);
            
            projectileSpawned = true;
        }

        if (timer > spawnDelay + timeOfFlight)
        {
            projectileInstance.Destroy();

            Destructible destructible = target.GetComponent<Destructible>();

            if (destructible != null)
            {
                destructible.Damage(CombatUtils.GetDamageToDestructible(destructible, baseDamage * damageModifer));
            }

            Done();
        }
    }
}


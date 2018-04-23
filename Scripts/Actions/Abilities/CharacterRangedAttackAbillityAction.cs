using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterRangedAttackAbillityAction : CharacterActionBase
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

    public CharacterRangedAttackAbillityAction(RangedAttackAbility ability, RangedWeapon weapon)
    {
        this.target = ability.target;
        this.velocity = ability.targetVelocity;
        this.timeOfFlight = ability.timeOfFlight;
        this.spawnDelay = ability.spawnDelay;
        this.projectile = ability.projectile;
        this.baseDamage = weapon.baseDamage + ability.baseDamage;
        this.damageModifer = ability.damageModifier;
        this.projectileSpawn = ability.weapon.projectileSpawn;
    }

    protected override void OnCharacterActionStart(Character character)
    {
        animator = character.animator;
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
            
            projectileSpawned = true;
        }

        if (timer > spawnDelay + timeOfFlight)
        {
            projectileInstance.Destroy();

            Destructible destructible = target.GetComponent<Destructible>();

            if(destructible != null) 
            {
                destructible.Damage(CombatUtils.GetDamageToDestructible(destructible, baseDamage * damageModifer));
            }

            Done();
        }
    }
}


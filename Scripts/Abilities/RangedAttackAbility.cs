using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Neverdawn/Abilities/Ranged", order = 1)]
public class RangedAttackAbility : CombatItemAbility
{
    [SerializeField]
    private Projectile _projectile;

    [SerializeField]
    private float _spawnDelay;

    [SerializeField]
    private float _damageModifier;

    [SerializeField]
    private float _baseDamage;

    [SerializeField]
    private SingleTargetProjectileCursor cursorPrefab;

    private SingleTargetProjectileCursor cursorInstance;


    /// <summary>
    /// Apply the value currently selected in the cursor;
    /// </summary>
    public override void ApplyCursor()
    {
        this.target = cursorInstance.target;
        this.targetVelocity = cursorInstance.targetVelocity;
        this.timeOfFlight = cursorInstance.timeOfFlight;
    }

    public override CharacterActionBase Cast()
    {
        base.Cast();

        CharacterActionGroup group = new CharacterActionGroup();

        Vector3 direction = targetVelocity;
        direction.y = 0.0f;
        direction.Normalize();

        group.PushAction(new CharacterTurnAction(direction));
        group.PushAction(new CharacterRangedAttackAbillityAction(this, weapon));

        return group;   
    }

    public override bool Initialize(GameObject gameObject)
    {
        this.weapon = gameObject.GetComponent<RangedWeapon>();

        if (weapon == null)
            return false;

        return base.Initialize(gameObject);
    }

    protected override AbilityCursorBase createCursor()
    {
        cursorInstance = Instantiate(cursorPrefab);
        cursorInstance.maxRange = weapon.range;
        cursorInstance.projectileOffset = weapon.projectileSpawn;

        return cursorInstance;
    }

    public RangedWeapon weapon { get; private set; }

    public Vector3 targetVelocity { get; set; }

    public float timeOfFlight { get; set; }

    public Frame target { get; set; }

    public Projectile projectile { get { return _projectile; } }

    public float spawnDelay { get { return _spawnDelay; } }

    public float damageModifier { get { return _damageModifier; } }

    public float baseDamage { get { return _baseDamage; } }

    public float threatRating { get; set; }
}

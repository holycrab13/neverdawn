using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Neverdawn/Abilities/Throw", order = 1)]
public class ThrowAbility : CombatItemAbility
{
    [SerializeField]
    private Projectile _projectile;

    [SerializeField]
    private float _spawnDelay;

    [SerializeField]
    private SingleTargetProjectileCursor cursorPrefab;


    private SingleTargetProjectileCursor cursorInstance;
   

    /// <summary>
    /// Apply the value currently selected in the cursor;
    /// </summary>
    public override void ApplyCursor()
    {
        this.target = cursorInstance.target;
        this.timeOfFlight = cursorInstance.timeOfFlight;
        this.targetVelocity = cursorInstance.targetVelocity;
    }


    public override CharacterActionBase Cast()
    {
        base.Cast();

        CharacterActionGroup group = new CharacterActionGroup();

        group.PushAction(new CharacterThrowAttackAbillityAction(this));

        return group;
    }

    public override bool Initialize(GameObject gameObject)
    {
        this.throwable = gameObject.GetComponent<Throwable>();

        if (throwable == null)
            return false;

        return base.Initialize(gameObject);
    }

    protected override AbilityCursorBase createCursor()
    {
        cursorInstance = Instantiate(cursorPrefab);
        cursorInstance.maxRange = 3 + (int)(0.1f * caster.GetAttributeLevel(AttributeType.Strength));
        cursorInstance.projectileOffset = throwable.throwOffset;
        return cursorInstance;
    }
    public float timeOfFlight { get; set; }

    public Frame target { get; set; }

    public Throwable throwable { get; private set; }

    public Vector3 targetVelocity { get; set; }

    public Projectile projectile { get { return _projectile; } }

    public float spawnDelay { get { return _spawnDelay; } }
}

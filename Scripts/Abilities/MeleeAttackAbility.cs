using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Neverdawn/Abilities/Melee", order = 1)]
public class MeleeAttackAbility : CombatItemAbility
{
    [SerializeField]
    private float _damageDelay;

    [SerializeField]
    private BuffBase[] _onHitBuffs;

    [SerializeField]
    private GameObject _onHitEffect;

    [SerializeField]
    private float _damageModifier;

    [SerializeField]
    private float _baseDamage;

    [SerializeField]
    private SingleTargetCursor meleeCursor;

    [SerializeField]
    private float _threatRating;

    private SingleTargetCursor cursorInstance;

    /// <summary>
    /// Apply the value currently selected in the cursor;
    /// </summary>
    public override void ApplyCursor()
    {
        this.target = cursorInstance.target;
        this.targetSelf = cursorInstance.targetSelf;
    }

    public override CharacterActionBase Cast()
    {
        base.Cast();

        CharacterActionGroup group = new CharacterActionGroup();

        if (!targetSelf)
        {
            group.PushAction(new CharacterTurnTowardsAction(target.position));
        }

        group.PushAction(new CharacterMeleeAttackAbilityAction(this));

        return group;   
    }

    public override bool Initialize(GameObject gameObject)
    {
        this.weapon = gameObject.GetComponent<Weapon>();

        if (weapon == null)
            return false;

        return base.Initialize(gameObject);
    }

    protected override AbilityCursorBase createCursor()
    {
        cursorInstance = Instantiate(meleeCursor);
        cursorInstance.maxRange = weapon.meleeRange;

        return cursorInstance;
    }

    public float damageDelay { get { return _damageDelay; } }

    public BuffBase[] onHitBuffs { get { return _onHitBuffs; } }

    public GameObject onHitEffect { get { return _onHitEffect; } }

    public float damageModifier { get { return _damageModifier; } }

    public float baseDamage { get { return _baseDamage; } }

    public float attackRange { get { return weapon.meleeRange; } }

    public Weapon weapon { get; private set; }

    public Frame target { get; set; }

    public bool targetSelf { get; set; }

    public float threatRating { get { return _threatRating; } }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Neverdawn/Abilities/ArcaneAoe", order = 1)]
public class ArcaneDamageAoeAbility : ArcaneAbility
{
    [SerializeField]
    private float _damageDelay;

    [SerializeField]
    private float _effectDelay;

    [SerializeField]
    private GameObject _aoeEffect;

    [SerializeField]
    private GameObject _onHitEffect;

    [SerializeField]
    private float _aoeRange;

    [SerializeField]
    private float _aoeDamage;

    /// <summary>
    /// Apply the value currently selected in the cursor;
    /// </summary>
    public override void ApplyCursor()
    {
        
    }

    public override CharacterActionBase Cast()
    {
        return new CharacterCastArcaneAoeAbility(this);
    }
  

    public float damageDelay { get { return _damageDelay; } }

    public float effectDelay { get { return _effectDelay; } }

    public GameObject onHitEffect { get { return _onHitEffect; } }

    public GameObject aoeEffect { get { return _aoeEffect; } }

    public float aoeRange { get { return _aoeRange; } }

    public float aoeDamage { get { return _aoeDamage; } }
}

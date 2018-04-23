using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Neverdawn/Abilities/BuffOnTarget", order = 1)]
public class ArcaneBuffOnTargetAbility : ArcaneAbility
{
    [SerializeField]
    private float _applyDelay;

    [SerializeField]
    private BuffBase[] _onHitBuffs;

    [SerializeField]
    private GameObject _onHitEffect;

    [SerializeField]
    private int _castRange;

    [SerializeField]
    private SingleTargetCursor singleTargetCursorPrefab;

    private SingleTargetCursor cursorInstance;

    public List<BuffBase> validBuffs { get; private set; }

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
        CharacterActionGroup group = new CharacterActionGroup();

        if (!targetSelf)
        {
            group.PushAction(new CharacterTurnTowardsAction(target.position));
        }

        group.PushAction(new CharacterCastBuffOnTargetAction(validBuffs, _onHitEffect, _applyDelay, target, targetSelf));

        return group;   
    }

    public override bool Initialize(GameObject gameObject)
    {
        validBuffs = new List<BuffBase>();

        foreach (ArcaneBuff buffPrefab in _onHitBuffs)
        {
            if (buffPrefab == null)
            {
                Debug.LogWarning("Buff is null on " + gameObject.name);
                continue;
            }

            ArcaneBuff buff = Instantiate(buffPrefab);

            if (buff.Initialize(gameObject))
            {
                validBuffs.Add(buff);
            }
            else
            {
                Debug.LogWarning("Failed to initialize " + buff.ToString());
                Destroy(buff);
            }
        }
        
        return base.Initialize(gameObject);
    }

    protected override AbilityCursorBase createCursor()
    {
        cursorInstance = Instantiate(singleTargetCursorPrefab);
        cursorInstance.maxRange = castRange;

        return cursorInstance;
    }

    public override string description
    {
        get
        {
            string result = base.description;

            foreach (BuffBase buff in validBuffs)
            {
                result += " " + buff.GenerateDescription();
            }

            return result;
        }
    }

    public float damageDelay { get { return _applyDelay; } }

    public GameObject onHitEffect { get { return _onHitEffect; } }

    public int castRange { get { return _castRange; } }

    public Frame target { get; set; }

    public bool targetSelf { get; set; }
}

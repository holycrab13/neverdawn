using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class CombatItemAbility : AbilityBase
{
    public CombatItem combatItem { get; protected set; }

    public sealed override CharacterActionBase Prepare()
    {
        return new CharacterDrawCombatItemAction(combatItem, this);
    }

    public sealed override CharacterActionBase Relax()
    {
        if (GameController.state != GameState.Combat)
        {
            return new CharacterSheathCombatItemAction();
        }

        return null;
    }

    public override bool Initialize(GameObject gameObject)
    {
        this.combatItem = gameObject.GetComponent<CombatItem>();

        return combatItem != null && base.Initialize(gameObject);
    }
}

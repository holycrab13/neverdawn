using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Neverdawn/Abilities/Consume", order = 1)]
public class ConsumeAbility : CombatItemAbility
{
    private Consumable consumable;

    [SerializeField]
    private float consumeDelay;

    public override bool Initialize(GameObject gameObject)
    {
        this.consumable = gameObject.GetComponent<Consumable>();

        return consumable != null && base.Initialize(gameObject);
    }

    public override CharacterActionBase Cast()
    {
        return new CharacterConsumeAction(consumable, consumeDelay);
    }

    public override string description
    {
        get
        {
            string description = base.description;

            foreach (BuffBase buff in consumable.buffs)
            {
                description += " " + buff.GenerateDescription();
            }

            return description;
        }
    }
}

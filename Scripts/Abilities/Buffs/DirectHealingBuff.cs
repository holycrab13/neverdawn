using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Direct Healing Buff", menuName = "Neverdawn/Buffs/DirectHealing", order = 1)]
public class DirectHealingBuff : ArcaneBuff {

    [SerializeField]
    private int restoreHealth;

    [SerializeField]
    private int restoreMana;

    [SerializeField]
    private GameObject effect;

    public override bool IsDone
    {
        get
        {
            return true;
        }
    }


    public override void UpdateBuff(Character character, float timekey)
    {
        Destructible destructible = character.GetComponent<Destructible>();

        if (destructible != null)
        {
            destructible.Heal(restoreHealth * arcaneObject.arcanePower);
        }

        Caster caster = character.GetComponent<Caster>();

        if(caster != null)
        {
            caster.manaPoints += restoreMana;
        }
        
    }

    public override string GenerateDescription()
    {
        return string.Format("Restores {0} health points.", (int)(restoreHealth * arcaneObject.arcanePower));
    }
}

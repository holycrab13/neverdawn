using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CombatUtils
{
    public static int GetDamageToDestructible(Destructible destructible, float amount)
    {
        Mannequin mannequin = destructible.GetComponent<Mannequin>();

        if(mannequin != null)
        {
            amount *= mannequin.armorMitigation;
        }

        return (int)amount;
    }
}
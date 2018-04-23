using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ArcaneAbility : CombatItemAbility
{
    public ArcaneObject arcaneObject { get; protected set; }

    public override bool Initialize(GameObject gameObject)
    {
        this.arcaneObject = gameObject.GetComponent<ArcaneObject>();

        return arcaneObject != null && base.Initialize(gameObject);
    }
}

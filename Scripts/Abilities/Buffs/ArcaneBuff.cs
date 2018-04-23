using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class ArcaneBuff : BuffBase
{
    public ArcaneObject arcaneObject { get; private set; }

    public override bool Initialize(GameObject gameObject)
    {
        arcaneObject = gameObject.GetComponent<ArcaneObject>();

        if (arcaneObject == null)
            return false;

        return base.Initialize(gameObject);
    }
}

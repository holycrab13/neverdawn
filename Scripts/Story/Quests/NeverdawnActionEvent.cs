using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class NeverdawnActionEvent : NeverdawnEventBase
{
    public abstract ActionBase GetAction();

    public override void UpdateEvent()
    {
        throw new NotImplementedException();
    }

    public override bool IsEventComplete()
    {
        throw new NotImplementedException();
    }

    public override void ResetEvent()
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DoNothing : CharacterActionBase
{

    public DoNothing()
    {

    }

    protected override void OnCharacterActionStart(Character actor)
    {
        Done();
    }

    public override void ActionUpdate(float timekey)
    {
       
    }
}


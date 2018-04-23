using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "New Trigger Event", menuName = "Neverdawn/Story/Trigger Event", order = 1)]
public class NeverdawnTriggerEvent : NeverdawnEventBase
{
    [SerializeField]
    private NeverdawnEventBase triggerEvent;

    private bool done;

    public override void UpdateEvent()
    {
        if (!done)
        {
            // GameController.TriggerEvent(triggerEvent);
            done = true;
        }
    }

    public override bool IsEventComplete()
    {
        return done;
    }

    public override void ResetEvent()
    {
        done = false;
    }

  
}

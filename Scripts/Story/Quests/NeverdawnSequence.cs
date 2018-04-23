using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

public class NeverdawnSequence : NeverdawnEventBase
{
    private NeverdawnEventBase currentEvent;

    private int index;

    public NeverdawnSequence(XmlNode node)
    {

    }

    public override void UpdateEvent()
    {


        if (currentEvent == null)
        {
            while (index < children.Count && !children[index].IsEventAvailable())
            {
                index++;
            }

            if (index < children.Count)
            {
                currentEvent = children[index];
            }
        }

        if (currentEvent != null)
        {
            if (!currentEvent.IsEventComplete())
            {
                currentEvent.UpdateEvent();
            }
            else
            {
                currentEvent = null;
                index++;
            }
        }
    }
      


    public override bool IsEventComplete()
    {
        return index >= children.Count;
    }

    public override void ResetEvent()
    {
        foreach (NeverdawnEventBase e in children)
        {
            e.ResetEvent();
        }

        index = 0;
    }

   

}

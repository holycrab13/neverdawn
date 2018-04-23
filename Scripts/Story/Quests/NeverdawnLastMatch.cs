using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class NeverdawnLastMatch : NeverdawnEventBase
{
    private NeverdawnEventBase currentEvent;

    private int index;
    private System.Xml.XmlNode node;

    public NeverdawnLastMatch(System.Xml.XmlNode node)
    {
        // TODO: Complete member initialization
        this.node = node;
    }

    public override void UpdateEvent()
    {
        if (currentEvent == null)
        {
            currentEvent = children.LastOrDefault(e => e.IsEventAvailable());
        }

        if (currentEvent != null)
        {
            if (currentEvent.IsEventComplete())
            {
                index = -1;
            }
            else
            {
                currentEvent.UpdateEvent();
            }
        }
        else
        {
            index = -1;
        }
    }


    public override bool IsEventComplete()
    {
        return index == -1;
    }

    public override void ResetEvent()
    {
        foreach (NeverdawnEventBase e in children)
        {
            e.ResetEvent();
        }

        currentEvent = null;
        index = 0;
    }
}

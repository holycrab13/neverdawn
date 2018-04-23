using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum EventExecutionType
{
    Sequence,
    Parallel,
    FirstMatch
}

[CreateAssetMenu(fileName = "New Event", menuName = "Neverdawn/Story/Event Switch", order = 1)]
public class NeverdawnEventSwitch : NeverdawnEventBase
{
    [SerializeField]
    private EventExecutionType mode;

    [SerializeField]
    private Discovery[] conditions;

    [SerializeField]
    private List<NeverdawnEventBase> events;

    private NeverdawnEventBase currentEvent;

    private int index;

    public override void UpdateEvent()
    {
        if (mode == EventExecutionType.Sequence)
        {
            if (currentEvent == null)
            {
                while (index < events.Count && !events[index].IsEventAvailable())
                {
                    index++;
                }

                if (index < events.Count)
                {
                    currentEvent = events[index];
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
        else if(mode == EventExecutionType.Parallel)
        {
            foreach(NeverdawnEventBase e in events)
            {
                if(e.IsEventAvailable() && !e.IsEventComplete())
                {
                    e.UpdateEvent();
                }
            }
        }
        else if(mode == EventExecutionType.FirstMatch)
        {
            if (currentEvent == null)
            {
                currentEvent = events.FirstOrDefault(e => e.IsEventAvailable());
            }

            if(currentEvent != null)
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
    }


    public override bool IsEventComplete()
    {
        if (mode == EventExecutionType.Sequence)
        {
            return index >= events.Count;
        }
        else if(mode == EventExecutionType.Parallel)
        {
            return events.TrueForAll(e => e.IsEventComplete() || !e.IsEventAvailable());
        }
        else if(mode == EventExecutionType.FirstMatch)
        {
            return index == -1;
        }

        return true;
    }

    public override void ResetEvent()
    {
        foreach (NeverdawnEventBase e in events)
        {
            e.ResetEvent();
        }

        index = 0;
    }

  
}

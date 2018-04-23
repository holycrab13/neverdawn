    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Xml;

public class EventController : MonoBehaviour
{
    [SerializeField]
    private TextAsset topicsFile;

    [SerializeField]
    private TextAsset discoveriesFile;

    [SerializeField]
    private UnityEvent onGoodbye;

    private List<AvatarController> controllers;

    private static EventController instance;

    private Frame target;

    private Queue<NeverdawnEventBase> eventQueue = new Queue<NeverdawnEventBase>();

    private NeverdawnEventBase currentEvent;

    private Dictionary<string, NeverdawnEventBase> events;

    private Dictionary<string, Topic> topics;


    private Dictionary<string, Discovery> discoveries;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }

    }

    internal static void ClearEvents()
    {
        instance.events.Clear();
    }

    public void Initialize()
    {
        loadEvents();
    }

    private void loadEvents()
    {
        events = new Dictionary<string, NeverdawnEventBase>();

        XmlDocument doc = NeverdawnScene.sceneEventsXml;

         XmlNodeList list = doc.GetElementsByTagName("events");

         if (list != null)
         {
             XmlNode eventsNode = list[0];

             if (eventsNode != null)
             {
                 foreach (XmlNode node in eventsNode.ChildNodes)
                 {
                     NeverdawnEventBase eventBase = NeverdawnUtility.LoadEvent(node);
                     events.Add(eventBase.id, eventBase);
                 }
             }
         }
    }
    
    public static Frame eventTarget
    {
        set { instance.target = value; }
        get { return instance.target; }
    }

    public void TriggerEvent(string trigger)
    {
 
        if (events.ContainsKey(trigger))
        {
            this.eventQueue.Enqueue(events[trigger]);
            this.controllers = GameController.activeControllers;
        }
    }
 
    public void UpdateEvent()
    {
        if (currentEvent == null)
        {
            if(eventQueue.Count > 0)
            {
                currentEvent = eventQueue.Dequeue();
                currentEvent.ResetEvent();
            }
            else
            {
                Goodbye();
            }
        }

        if(currentEvent != null)
        {
            if(!currentEvent.IsEventComplete())
            {
                currentEvent.UpdateEvent();
            }
            else
            {
                currentEvent = null;
            }
        }

      
    }

    internal static void Goodbye()
    {
        instance.onGoodbye.Invoke();
    }

    //public static Frame GetTarget(NeverdawnEventTargetType targetType)
    //{
    //    switch (targetType)
    //    {
    //        case NeverdawnEventTargetType.Self0:
    //            return instance.getTargetSelf(0);
    //        case NeverdawnEventTargetType.Self1:
    //            return instance.getTargetSelf(1);
    //        case NeverdawnEventTargetType.Self2:
    //            return instance.getTargetSelf(2);
    //        case NeverdawnEventTargetType.Self3:
    //            return instance.getTargetSelf(3);
    //        case NeverdawnEventTargetType.Target:
    //            return instance.getCurrentTarget();
    //        case NeverdawnEventTargetType.Identifier:
    //            return instance.getTargetByIdentifier();
    //        case NeverdawnEventTargetType.None:
    //            return null;
    //    }

    //    return null;
    //}

    private Frame getTargetByIdentifier()
    {
        return null;
    }

    private Frame getCurrentTarget()
    {
        return target;
    }

    public static Discovery GetDiscovery(string key)
    {
        return instance.discoveries[key];
    }

    internal static NeverdawnEventBase FindEvent(string p)
    {
        if(instance.events.ContainsKey(p))
        {
            return instance.events[p];
        }

        return null;
    }

}

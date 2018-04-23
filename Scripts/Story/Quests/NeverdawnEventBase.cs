using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;

public enum NeverdawnEventTargetType
{
    Self0,
    Self1,
    Self2,
    Self3,
    Target,
    Identifier,
    None
}


public abstract class NeverdawnEventBase
{
    private static string EVENT_TYPE_SEQUENCE = "sequence";

    private static string EVENT_TYPE_FIRST_MATCH = "firstmatch";

    private static string EVENT_TYPE_LAST_MATCH = "lastmatch";

    private static string EVENT_TYPE_PARALLEL = "parallel";

    private static string EVENT_TYPE_PHRASE = "phrase";

    private static string EVENT_TYPE_INVESTIGATION = "investigation";

    private static string EVENT_TYPE_QUESTION = "question";

    private static string EVENT_TYPE_ANSWER = "answer";

    private static string EVENT_TYPE_DISCOVERY = "discovery";

    private static string EVENT_TYPE_TRIGGER = "trigger";

    private static Regex EVENT_TARGET_SELF = new Regex("self[0-9]");

    private static Regex EVENT_TARGET_TARGET = new Regex("target");

    public string[] conditions { get; private set; }

    public string id { get; private set; }

    private string targetId;

    public string[] inverseConditions { get; private set; }

    public List<NeverdawnEventBase> children { get; private set; }

    public abstract void UpdateEvent();

    public abstract bool IsEventComplete();

    public virtual void ResetEvent()
    {

    }

    internal static NeverdawnEventBase LoadEvent(XmlNode node)
    { 
        string eventType = node.Name;
        NeverdawnEventBase result = null;

        if (eventType.Equals(EVENT_TYPE_SEQUENCE))
        {
             result = new NeverdawnSequence(node);
        }

        if (eventType.Equals(EVENT_TYPE_FIRST_MATCH))
        {
            result = new NeverdawnFirstMatch(node);
        }

        if (eventType.Equals(EVENT_TYPE_LAST_MATCH))
        {
            result = new NeverdawnLastMatch(node);
        }

        if (eventType.Equals(EVENT_TYPE_PHRASE))
        {
            result = new NeverdawnPhrase(node);
        }

        if (eventType.Equals(EVENT_TYPE_INVESTIGATION))
        {
            result = new NeverdawnInvestigateEvent();
        }

        if(eventType.Equals(EVENT_TYPE_DISCOVERY))
        {
            result = new NeverdawnDiscovery(node);
        }
    
        if (result != null)
        {
            result.id = XmlUtil.Get(node, "id", string.Empty);
            result.conditions = XmlUtil.GetArray(node, "if", ' ');
            result.inverseConditions = XmlUtil.GetArray(node, "not", ' ');
            result.targetId = XmlUtil.Get(node, "target", string.Empty);
            result.children = new List<NeverdawnEventBase>();
        }

        return result;
    }

    public virtual bool IsEventAvailable()
    {
        return PlayerJournal.HasDiscoveries(conditions, inverseConditions);
    }

    public Frame target
    {
        get 
        {
            if (EVENT_TARGET_SELF.IsMatch(targetId))
            {
                int index = int.Parse(targetId.Substring(targetId.Length -1));
                return getTargetSelf(index).frame;
            }

            if(EVENT_TARGET_TARGET.IsMatch(targetId))
            {
                return EventController.eventTarget;
            }

            return Frame.FindFrameById(targetId);
        }
    }

    protected Character getTargetSelf(int p)
    {

        if (GameController.instance.party.Count == 0)
        {
            Debug.LogWarning("Event without characters, something went completely wrong!");
            return null;
        }

        return GameController.instance.party[Mathf.Min(GameController.instance.party.Count - 1, p)];
    }


    protected string createMessage(string message)
    {
        string targetName = string.Empty;

        if (EventController.eventTarget != null && EventController.eventTarget.GetComponent<Identity>() != null)
        {
            targetName = EventController.eventTarget.GetComponent<Identity>().label;
        }

        return message.Replace("{t}", targetName);
    }
}

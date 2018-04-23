using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;
using System;

[Serializable]
public class JournalEntry : ITransmittable
{
    public string header;
    public string message;

    public void ReadData(IMessageReader reader)
    {
        header = reader.ReadString();
        message = reader.ReadString();
    }

    public void WriteData(IMessageWriter writer)
    {
        writer.WriteString(header);
        writer.WriteString(message);
    }
}

public class PlayerJournal : FrameComponent
{
    private List<JournalEntry> _entries;

    [SerializeField]
    private List<string> _topics;

    [SerializeField]
    private List<string> _discoveries;

    private static PlayerJournal instance;

    public static IEnumerable<JournalEntry> entries
    {
        get { return instance._entries; }
    }

   
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        _entries = new List<JournalEntry>();
    }

    protected override void readData(IMessageReader reader)
    {
        _discoveries = reader.ReadStringArray().ToList();
        _topics = reader.ReadStringArray().ToList();

        int numberOfEntries = reader.ReadInt();
        _entries = new List<JournalEntry>();

        for(int i = 0; i < numberOfEntries; i++)
        {
            JournalEntry entry = new JournalEntry();
            entry.ReadData(reader);

            _entries.Add(entry);
        }
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteStringArray(_discoveries.ToArray());
        writer.WriteStringArray(_topics.ToArray());
        writer.WriteInt(_entries.Count);

        foreach(JournalEntry entry in _entries)
        {
            writer.WriteTransmittable(entry);
        }
    }

    internal static void AddTopic(Topic topic)
    {
        instance._topics.Add(topic.id);
    }

    internal static void AddTopics(IEnumerable<Topic> topicsToAdd)
    {
        instance._topics.AddRange(topicsToAdd.Select(t => t.id));
    }

    internal static void Note(Response response)
    {
       
    }

    private void unlock(params string[] discoveriesToUnlock)
    {
        foreach (string discovery in discoveriesToUnlock)
        {
            Debug.Log("Unlocked discovery: " + discovery);
            _discoveries.Add(discovery);
        }
    }

    internal static bool HasDiscoveries(IEnumerable<string> conditions, IEnumerable<string> inverseConditions = null)
    {
        foreach (string discovery in conditions)
        {
            if (!instance._discoveries.Contains(discovery))
            {
                return false;
            }
        }

        if (inverseConditions != null)
        {
            foreach (string discovery in inverseConditions)
            {
                if (instance._discoveries.Contains(discovery))
                {
                    return false;
                }
            }
        }

        return true;
    }

    internal static Topic[] GetCommonTopics(Topic[] topicsOther)
    {
        List<Topic> commonTopics = new List<Topic>();

        foreach(Topic otherTopic in topicsOther)
        {
            if (instance._topics.Contains(otherTopic.id))
            {
                commonTopics.Add(otherTopic);
            }
        }

        return commonTopics.ToArray();
    }

    internal static void CreateEntry(Discovery discovery, Frame target)
    {   
        if (!instance._discoveries.Contains(discovery.id))
        {
            instance.unlock(discovery.id);

            string targetName = target != null ? target.label : string.Empty;

            instance._entries.Add(new JournalEntry()
            {
                header = PlayerTime.GetTimeString() + ", " + targetName,
                message = discovery.GetText(targetName)
            });
        }
    }
}

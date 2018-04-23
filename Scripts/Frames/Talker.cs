using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public struct TopicQuestion 
{
    public string topicId;
    public string message;
    public string trigger;

    internal static TopicQuestion Create(Talker talker, Topic topic)
    {
        TopicQuestion result = new TopicQuestion();
        result.message = topic.questionMessage;
        result.topicId = topic.id;
        result.trigger = string.Empty;

        if (talker.HasCustomQuestion(topic))
        {
            TopicQuestion customQuestion = talker.GetCustomQuestion(topic);

            if (!string.IsNullOrEmpty(customQuestion.message))
            {
                result.message = customQuestion.message;
            }

            if (customQuestion.trigger != null)
            {
                result.trigger = customQuestion.trigger;
            }
        }

        return result;
    }
}

[AddComponentMenu("FrameComponent/Talker")]
public class Talker : FrameComponent
{
    [SerializeField]
    private string trigger;

    [SerializeField]
    private TopicQuestion[] topicQuestions;

    public string[] topicIds { get; private set; }

    /// <summary>
    /// Checks whether any topics
    /// </summary>
    /// <param name="topic"></param>
    /// <returns></returns>
    public bool HasCustomQuestion(Topic topic)
    {
        return topicQuestions.Any(t => t.topicId == topic.id);
    }

    public TopicQuestion GetCustomQuestion(Topic topic)
    {
        return topicQuestions.FirstOrDefault(t => t.topicId == topic.id);
    }

    void Start()
    {
        topicIds = topicQuestions.Select(t => t.topicId).ToArray();
        // topics = EventController.GetTopics(topicIds, true);
    }
   
    public void Talk(AvatarController controller)
    {
        Vector3 dir = (transform.position - controller.character.transform.position).normalized;
        controller.character.PushAction(new CharacterTurnAction(dir, 90.0f));

        Character npc = GetComponent<Character>();

        if (npc != null)
        {
            npc.PushAction(new CharacterTurnAction(-dir, 90.0f));
        }

        if (!string.IsNullOrEmpty(trigger))
        {
            EventController.eventTarget = frame;
            GameController.instance.TriggerEvent(trigger);
        }
    }
}

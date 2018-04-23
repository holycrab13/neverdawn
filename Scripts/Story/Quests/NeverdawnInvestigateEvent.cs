using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class NeverdawnInvestigateEvent : NeverdawnEventBase
{
    private QuestionMessage chatMessage;

    private List<AvatarController> controllers;

    private NeverdawnEventBase selectedEvent;

    public NeverdawnInvestigateEvent()
    {

    }

    public override void UpdateEvent()
    {
        if (selectedEvent != null && !selectedEvent.IsEventComplete())
        {
            selectedEvent.UpdateEvent();
            return;
        }

        if (chatMessage == null)
        {
            if(target != null)
            {
                Talker talker = target.GetComponent<Talker>();

                if (talker != null)
                {
                    Topic[] talkerTopics = NeverdawnDatabase.GetTopics(talker.topicIds, true);
                    Topic[] talkTopics = PlayerJournal.GetCommonTopics(talkerTopics);
                    TopicQuestion[] questions = new TopicQuestion[talkTopics.Length + 1];

                    for (int i = 0; i < talkTopics.Length; i++)
                    {
                        questions[i] = TopicQuestion.Create(talker, talkTopics[i]);
                    }

                    questions[questions.Length - 1] = new TopicQuestion()
                    {
                        message = "That'll be all.",
                        trigger = null
                    };
                   

                    chatMessage = new QuestionMessage(getTargetSelf(0).identity.icon, string.Empty, questions);
                    UIChatMenu.SendQuestionMessage(chatMessage, questionSelected);
                }
            }
        }
    }

    private void questionSelected(TopicQuestion question)
    {
        if (question.trigger == null)
        {
            chatMessage.Discard();
        }
        else
        {
            chatMessage = null;

            if (question.trigger.Equals(string.Empty))
            {
                Topic topic = NeverdawnDatabase.GetTopic(question.topicId);
                selectedEvent = topic.defaultEvent;
            }
            else
            {
                selectedEvent = EventController.FindEvent(question.trigger);
            }

            selectedEvent.ResetEvent();
        }
    }

    private void close()
    {
        if (chatMessage != null)
        {
            chatMessage.Discard();
            UIChatMenu.Hide();
        }
    }

    public override bool IsEventComplete()
    {
        return chatMessage != null && chatMessage.discarded;
    }

    public override void ResetEvent()
    {
        chatMessage = null;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "New Question Event", menuName = "Neverdawn/Story/Question Event", order = 1)]
public class NeverdawnQuestionEvent : NeverdawnEventBase
{
    [SerializeField]
    private string message;

    [SerializeField]
    private TopicQuestion[] respones;

    private QuestionMessage chatMessage;

    private List<AvatarController> controllers;

    public override void UpdateEvent()
    {
        if (chatMessage == null)
        {
            Character chatTarget = getTargetSelf(0);
            chatMessage = new QuestionMessage(chatTarget.identity.icon, message, respones);
            UIChatMenu.SendQuestionMessage(chatMessage);
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


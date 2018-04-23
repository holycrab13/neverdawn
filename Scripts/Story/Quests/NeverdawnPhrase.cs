using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;


public class NeverdawnPhrase : NeverdawnEventBase
{
    private string message;
    
    private ChatMessage chatMessage;

    private bool invalid;

    private List<AvatarController> controllers;

    public NeverdawnPhrase(XmlNode node)
    {
        message = node.InnerText;
    }

    public override void UpdateEvent()
    {
        if (chatMessage == null)
        {
            Character chatTarget = target.GetComponent<Character>();

            if(chatTarget == null)
            {
                chatMessage = new ChatMessage(null, createMessage(message), ChatMessageIconMode.None);
                UIChatMenu.SendChatMessage(chatMessage);
                return;
            }

            ChatMessageIconMode mode = GameController.instance.party.IsCharacterInParty(chatTarget) ?
                ChatMessageIconMode.Right : ChatMessageIconMode.Left;

            chatMessage = new ChatMessage(chatTarget.identity.icon, createMessage(message), mode);
            UIChatMenu.SendChatMessage(chatMessage);
        }

        if(controllers == null || controllers.Count == 0)
        {
            controllers = GameController.activeControllers;
        }
    
        foreach (AvatarController controller in controllers)
        {
            if (controller.inputModule.GetButtonDown(NeverdawnInputButton.Confirm))
            {
                close();
            }

            if (controller.inputModule.GetButtonDown(NeverdawnInputButton.Cancel))
            {
                close();
            }
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
        return invalid || (chatMessage != null && chatMessage.discarded);
    }

    public override void ResetEvent()
    {
        controllers = new List<AvatarController>();
        chatMessage = null;
        invalid = false;
    }

  
}

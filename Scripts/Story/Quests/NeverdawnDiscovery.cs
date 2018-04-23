using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;


public class NeverdawnDiscovery : NeverdawnEventBase
{
    private static string updatedMessage = "Your journal has been updated!";

    private string[] discoveries;
    
    private ChatMessage chatMessage;

    private List<AvatarController> controllers;

    public NeverdawnDiscovery(XmlNode node) 
    {
        discoveries = XmlUtil.GetArray(node, "discoveries", ' ');
    }

   public override void UpdateEvent()
    {
        if (chatMessage == null)
        {
            foreach(string discoveryId in discoveries) {

                Discovery discovery = NeverdawnDatabase.GetDisovery(discoveryId);

                PlayerJournal.CreateEntry(discovery, target);
            }

            chatMessage = new ChatMessage(null, updatedMessage, ChatMessageIconMode.None);
            UIChatMenu.SendChatMessage(chatMessage);
            return;
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

    public override bool IsEventAvailable()
    {
        return !PlayerJournal.HasDiscoveries(discoveries) && base.IsEventAvailable();
    }

    public override bool IsEventComplete()
    {
        return (chatMessage != null && chatMessage.discarded);
    }

    public override void ResetEvent()
    {
        controllers = new List<AvatarController>();
        chatMessage = null;
    }
}

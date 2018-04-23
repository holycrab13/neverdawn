using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChatMenu : MonoBehaviour {

    [SerializeField]
    private UIChatPanel chatPanelTalker;

    [SerializeField]
    private UIChatPanel chatPanelCharacter;

    [SerializeField]
    private UIChatPanel chatPanelNoIcon;

    [SerializeField]
    private UIQuestionPanel questionPanel;

    private static UIChatMenu instance;

    private int selectionIndex;

    private static List<UITextButton> questionButtons;

    private Sprite characterIcon;

    private Talker talker;
    private ChatMessage chatMessage;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        instance.gameObject.SetActive(false);
    }

    public static void Show(Talker talker, AvatarController controller)
    {
        instance.talker = talker;
        instance.gameObject.SetActive(true);

        instance.characterIcon = controller.character.identity.icon;
    }

    private static void chat(NeverdawnEventSwitch question)
    {
        //instance.sendCharacterMessage(question.question);
        //instance.sendTalkerMessage(question.TriggerAnswer());
    }

    private static void goodbye()
    {
        EventController.Goodbye();
    }

    private void sendRightChatMessage(Sprite icon, string p)
    {
        chatPanelCharacter.gameObject.SetActive(true);

        chatPanelTalker.gameObject.SetActive(false); 
        chatPanelNoIcon.gameObject.SetActive(false);
        questionPanel.gameObject.SetActive(false);

        chatPanelCharacter.icon = icon;
        chatPanelCharacter.message = p;
    }

    private void sendLeftChatMessage(Sprite icon, string p)
    {
        chatPanelTalker.gameObject.SetActive(true);

        chatPanelCharacter.gameObject.SetActive(false);
        chatPanelNoIcon.gameObject.SetActive(false);
        questionPanel.gameObject.SetActive(false);

        chatPanelTalker.icon = icon;
        chatPanelTalker.message = p;
    }

    private void sendChatMessageNoIcon(string p)
    {
        chatPanelNoIcon.gameObject.SetActive(true);

        chatPanelCharacter.gameObject.SetActive(false);
        chatPanelTalker.gameObject.SetActive(false);
        questionPanel.gameObject.SetActive(false);

        chatPanelNoIcon.message = p;
    }

    private void sendQuestionMessage(QuestionMessage chatMessage, Action<TopicQuestion> selectionCallback = null)
    {
        questionPanel.gameObject.SetActive(true);

        chatPanelCharacter.gameObject.SetActive(false);
        chatPanelTalker.gameObject.SetActive(false);
        chatPanelNoIcon.gameObject.SetActive(false);

        questionPanel.selectionCallback = selectionCallback;
        questionPanel.message = chatMessage;
    }

    public static void Hide()
    {
        instance.gameObject.SetActive(false);
    }

    internal static void SendChatMessage(ChatMessage chatMessage)
    {
        instance.gameObject.SetActive(true);
     
        if (chatMessage.mode == ChatMessageIconMode.Left)
        {
            instance.sendLeftChatMessage(chatMessage.icon, chatMessage.message);
        }

        if (chatMessage.mode == ChatMessageIconMode.Right)
        {
            instance.sendRightChatMessage(chatMessage.icon, chatMessage.message);
        }

        if(chatMessage.mode == ChatMessageIconMode.None)
        {
            instance.sendChatMessageNoIcon(chatMessage.message);
        }
    }

    internal static void SendQuestionMessage(QuestionMessage chatMessage, Action<TopicQuestion> selectionCallback = null)
    {
        instance.gameObject.SetActive(true);
        instance.sendQuestionMessage(chatMessage, selectionCallback);
    }

}

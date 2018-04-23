using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIQuestionPanel : MonoBehaviour {

    [SerializeField]
    private UITextReveal text;

    [SerializeField]
    private Image image;

    [SerializeField]
    private UITextButton textButtonPrefab;

    [SerializeField]
    private RectTransform textButtonParent;

    [SerializeField]
    private UIInputController inputController;

    private QuestionMessage _message;

    public Sprite icon
    {
        set { image.sprite = value; }
    }


    public System.Action<TopicQuestion> selectionCallback { get; set; }

    public QuestionMessage message
    {
        set 
        {
            _message = value;
            icon = value.icon;

            inputController.SetInputModules(GameController.activeControllers.Select(c => c.inputModule).ToArray());
           
            foreach (Transform child in textButtonParent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            List<UITextButton> buttons = new List<UITextButton>();

            foreach (TopicQuestion question in value.questions)
            {
                UITextButton button = Instantiate(textButtonPrefab);
                button.transform.SetParent(textButtonParent, false);
                button.text = question.message;
                button.onClick.AddListener(() => responseSelected(question));

                buttons.Add(button);
            }

            if (buttons.Count > 0)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].neighborTop = buttons[NeverdawnUtility.RepeatIndex(i - 1, buttons.Count)];
                    buttons[i].neighborBottom = buttons[NeverdawnUtility.RepeatIndex(i + 1, buttons.Count)];
                }

                buttons[0].Select();
            }
        }
    }

    private void responseSelected(TopicQuestion response)
    {
        if (selectionCallback != null)
        {
            selectionCallback(response);
        }
        else
        {
            GameController.instance.TriggerEvent(response.trigger);
            UIChatMenu.Hide();
            _message.Discard();
        }
    }


}

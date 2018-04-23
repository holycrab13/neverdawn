using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIQuickMenuCombatItem : UIQuickMenuPage
{
    public CombatItem combatItem { get; set; }

    [SerializeField]
    private RectTransform buttonParent;

    [SerializeField]
    private TMP_Text label;

    [SerializeField]
    private Sprite iconCancel;

    [SerializeField]
    private string labelCancel = "Go Back";

    private int selectedIndex;

    [SerializeField]
    private List<UIIconButton> buttons;

    protected override void OnQuickMenuPageEnabled()
    {
        if (buttons.Count > selectedIndex)
        {
            buttons[selectedIndex].Select();
        }
    }

    void Start()
    {
        buttons = new List<UIIconButton>();

        foreach (AbilityBase ability in combatItem.abilities)
        {
            UIAbilityIconButton button = Instantiate(UIFactory.uiAbilityButton);
            button.ability = ability;
            button.menu = menu;
            button.label = label;

            buttons.Add(button);
        }

        UIIconButton buttonCancel = Instantiate(UIFactory.uiIconButton);
        buttonCancel.SetIconSprite(iconCancel);
        buttonCancel.onSelect.AddListener(() => label.text = labelCancel);
        buttonCancel.onClick.AddListener(() => menu.GoBack());
        buttons.Add(buttonCancel);

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].transform.SetParent(buttonParent);
            buttons[i].neighborLeft = buttons[NeverdawnUtility.RepeatIndex(i - 1, buttons.Count)];
            buttons[i].neighborRight = buttons[NeverdawnUtility.RepeatIndex(i + 1, buttons.Count)];
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickMenuJournal : UIQuickMenuPage
{
    [SerializeField]
    private UITextButton journalEntryPrefab;

    [SerializeField]
    private RectTransform journalEntryParent;

    private int selectedIndex;

    private List<UIButton> buttons;

    [SerializeField]
    private UIListSelectionController listController;

    private Dictionary<AttributeType, UISkillView> attributeViewMap;

    private Dictionary<SkillType, UISkillView> skillViewMap;

	// Use this for initialization
    void Start()
    {
        buttons = new List<UIButton>();
        attributeViewMap = new Dictionary<AttributeType, UISkillView>();
        skillViewMap = new Dictionary<SkillType, UISkillView>();

        Character character = avatarController.character;

        listController.inputModule = avatarController.inputModule;

        int k = 0;

        foreach (JournalEntry entry in PlayerJournal.entries)
        {
            UITextButton textButton = Instantiate(journalEntryPrefab);
            int i = k++;

            textButton.transform.SetParent(journalEntryParent);
            textButton.text = entry.header;

            textButton.onClick.AddListener(() => entrySelected(entry, i));

            buttons.Add(textButton);
        }

        selectedIndex = 0;
    }

    private void entrySelected(JournalEntry entry, int i)
    {
        
    }


    protected override void OnQuickMenuPageEnabled()
    {
        Character character = avatarController.character;

        selectedIndex = NeverdawnUtility.RepeatIndex(selectedIndex, buttons.Count);

        if (buttons.Count > 0)
        {
            buttons[selectedIndex].Select();
        }
    }
}

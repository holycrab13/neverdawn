using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMenuPlacement : UIQuickMenuPage
{
    [SerializeField]
    private UIIconButton centerButton;

    [SerializeField]
    private UIIconButton iconButtonPrefab;

    [SerializeField]
    private Transform iconButtonParent;

    private Dictionary<UISelectable, Character> buttonMap;

    private int characterCount;

    void Update()
    {
        if (PlacementController.charactersToPlace.Count != characterCount)
        {
            characterCount = PlacementController.charactersToPlace.Count;

            if (characterCount == 0)
            {
                menu.Close();
            }
            else
            {
                updatePage();
            }
        }
    }

    private void updatePage()
    {
        List<UIIconButton> buttons = new List<UIIconButton>();
        buttons.Add(centerButton);

        buttonMap = new Dictionary<UISelectable, Character>();

        foreach (Character character in PlacementController.charactersToPlace)
        {
            Character c = character;
            UIIconButton iconButton = Instantiate(iconButtonPrefab);
            iconButton.SetIconSprite(character.identity.icon);
            iconButton.transform.SetParent(iconButtonParent);
            iconButton.onClick.AddListener(() => click(c));

            buttons.Add(iconButton);
        }

        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].neighborLeft = buttons[NeverdawnUtility.RepeatIndex(i - 1, buttons.Count)];
            buttons[i].neighborRight = buttons[NeverdawnUtility.RepeatIndex(i + 1, buttons.Count)];
        }

        buttons[0].Select();
    }


    private void click(Character character)
    {
        menu.Close();
        avatarController.PlaceCharacter(character);
    }
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class UIQuickMenuCombat : UIQuickMenuPage {

    public Character character { get; set; }

    public bool includeWalk { get; set; }

    public bool includeDoNothing { get; set; }

    [SerializeField]
    private RectTransform buttonParent;

    [SerializeField]
    private TMP_Text label;

    [SerializeField]
    private UIIconButton buttonPrefab;

    [SerializeField]
    private Sprite iconBack;

    [SerializeField]
    private Sprite iconDoNothing;

    [SerializeField]
    private string labelBack;

    [SerializeField]
    private string labelDoNothing = "Do Nothing";

    [SerializeField]
    private string labelUnarmed = "Unarmed";

    [SerializeField]
    private List<UIIconButton> buttons;

    private int selectedIndex;

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

        if (character.mannequin)
        {
            foreach (CombatItem item in character.mannequin.GetEquippedCombatItems())
            {
                UICombatItemIconButton combatItemButton = Instantiate(UIFactory.uiCombatItemButton);
                combatItemButton.combatItem = item;
                combatItemButton.menu = menu;
                combatItemButton.label = label;
                buttons.Add(combatItemButton);
            }
        }

        if (character.unarmed)
        {
            UICombatItemIconButton combatItemButton = Instantiate(UIFactory.uiCombatItemButton);
            combatItemButton.combatItem = character.unarmed;
            combatItemButton.menu = menu;
            combatItemButton.label = label;
            combatItemButton.customLabel = "Self";
            buttons.Add(combatItemButton);
        }

        if (includeWalk)
        {
            AbilityBase walkAbility = Instantiate(CombatController.walkAbility);
            walkAbility.Initialize(character.gameObject);

            UIAbilityIconButton walkButton = Instantiate(UIFactory.uiAbilityButton);
            walkButton.ability = walkAbility;
            walkButton.menu = menu;
            walkButton.label = label;
            buttons.Add(walkButton);
        }

        if (includeDoNothing)
        {
            UIIconButton buttonDoNothing = Instantiate(UIFactory.uiIconButton);
            buttonDoNothing.SetIconSprite(iconDoNothing);
            buttonDoNothing.onSelect.AddListener(() => label.text = labelDoNothing);
            buttonDoNothing.onClick.AddListener(() => doNothing());
            buttons.Add(buttonDoNothing);
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].transform.SetParent(buttonParent);
            buttons[i].neighborLeft = buttons[NeverdawnUtility.RepeatIndex(i - 1, buttons.Count)];
            buttons[i].neighborRight = buttons[NeverdawnUtility.RepeatIndex(i + 1, buttons.Count)];
        }
    }

    private void doNothing()
    {
        character.remainingSteps = 0;
        character.remainingActions = 0;
        menu.Close();
    }
}

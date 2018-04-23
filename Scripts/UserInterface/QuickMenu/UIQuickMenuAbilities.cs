using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UIQuickMenuAbilities : UIQuickMenuPage {

    [SerializeField]
    private UITintedIconButton iconButtonPrefab;

    [SerializeField]
    private RectTransform iconButtonParent;

    [SerializeField]
    private UIGridSelectionController gridController;

    [SerializeField]
    private Text textAbilityLabel;

    [SerializeField]
    private UIQuickMenuAbilityView abilityViewPrefab;

    private int selectionIndex;

    private AbilityBase[] abilities;

    private Character character;

    /// <summary>
    /// Setup grid controller and listen for item changes
    /// </summary>
    void Start()
    {
        gridController.onSelectionChanged.AddListener(selectionChanged);
        gridController.inputModule = avatarController.inputModule;

        character = avatarController.character;

        IEnumerable<AbilityBase> characterAbilities = character.GetAllAbilities();

        abilities = characterAbilities.ToArray(); // characterAbilities.Where(a => a.usableOutOfCombat).ToArray();

        updateView();
    }


    protected override void OnQuickMenuPageEnabled()
    {
        updateSelection();
    }

    private void updateView()
    {
        foreach (Transform child in iconButtonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (AbilityBase ability in abilities)
        {
            UITintedIconButton button = Instantiate(iconButtonPrefab);

            button.SetIconSprite(ability.icon);
            button.onClick.AddListener(() => abilityClicked(ability));
            button.transform.SetParent(iconButtonParent, false);
            button.tinted = !ability.IsCastable(character);
        }

        Invoke("updateSelection", 0.0f);
    }

    void updateSelection()
    {
        selectionIndex = Mathf.Clamp(selectionIndex, 0, gridController.length - 1);
        gridController.Select(selectionIndex);
    }

    private void selectionChanged()
    {
        if (abilities.Length == 0)
        {
            textAbilityLabel.text = string.Empty;
        }
        else
        {
            selectionIndex = gridController.selectedIndex;

            AbilityBase currentAbility = abilities[selectionIndex];
            textAbilityLabel.text = currentAbility.label;
   
        }
    }

    private void abilityClicked(AbilityBase ability)
    {
        UIQuickMenuAbilityView instance = Instantiate(abilityViewPrefab);
        instance.ability = ability;

        menu.NavigateInto(instance);
    }
   
}

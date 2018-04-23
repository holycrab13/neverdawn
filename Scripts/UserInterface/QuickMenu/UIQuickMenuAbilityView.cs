using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIQuickMenuAbilityView : UIQuickMenuPage {

    [SerializeField]
    private Text textLabel;

    [SerializeField]
    private Text textDescription;

    [SerializeField]
    private Text textError;

    [SerializeField]
    private Image imageIcon;

    [SerializeField]
    private UIIconButton castButton;

    [SerializeField]
    private UIIconButton quickSlotButton;

    [SerializeField]
    private UIIconButton cancelButton;

    [SerializeField]
    private UIQuickMenuPickQuickSlot pickQuickSlotPrefab;

	// Use this for initialization
	void Start () {
		
        character = avatarController.character;

        castButton.interactable = ability.IsCastable(character);
        quickSlotButton.interactable = ability.IsCastable(character);

        textLabel.text = ability.label;
        textDescription.text = ability.description;
        textError.text = createErrorText(character, ability);
        imageIcon.sprite = ability.icon;

	}

    protected override void OnQuickMenuPageEnabled()
    {
        castButton.Select();
    }

    private string createErrorText(Character character, AbilityBase ability)
    {
        List<string> errors = new List<string>();

        if (!ability.CharacterHasEnoughMana(character))
            errors.Add("not enough mana");

        if (!ability.CharacterHasSkill(character))
            errors.Add(ability.GetRequirementString(character));

        if (!ability.usableOutOfCombat)
            errors.Add(string.Format("cannot use out of combat", ability.label));

        string errorString = string.Join(", ", errors.ToArray());

        if (!string.IsNullOrEmpty(errorString))
        {
            errorString = errorString.First().ToString().ToUpper() + errorString.Substring(1);
        }

        return errorString;
    }

    public void Cancel()
    {
        menu.GoBack();
    }
	
    public void Cast()
    {
        avatarController.CastAbility(ability);
    }

    public void QuickSlotThis()
    {
        UIQuickMenuPickQuickSlot instance = Instantiate(pickQuickSlotPrefab);
        instance.ability = ability;

        menu.NavigateInto(instance);
    }

    public AbilityBase ability { get; set; }

    public Character character { get; set; }
}

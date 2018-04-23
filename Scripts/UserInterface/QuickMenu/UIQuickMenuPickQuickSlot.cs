using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectionControllerBase))]
public class UIQuickMenuPickQuickSlot : UIQuickMenuPage
{

    [SerializeField]
    private UIIconButton[] iconButtons;

    [SerializeField]
    private Sprite emptySprite;

    private Character character;

	// Use this for initialization
	void Start () {

        GetComponent<SelectionControllerBase>().inputModule = avatarController.inputModule;

        character = avatarController.character;

        for (int i = 0; i < character.quickCastAbilities.Length; i++)
        {
            AbilityBase ability = character.quickCastAbilities[i];

            if(ability != null)
            {
                iconButtons[i].SetIconSprite(ability.icon);
            }

            int k = i;
            UIIconButton button = iconButtons[i];
            button.onSelect.AddListener(() => setIcon(button));
            button.onDeSelect.AddListener(() => resetIcon(k));
        }
	}

    private void resetIcon(int k)
    {
        Sprite icon = character.quickCastAbilities[k] != null ? character.quickCastAbilities[k].icon : emptySprite;
        iconButtons[k].SetIconSprite(icon);
    }

    private void setIcon(UIIconButton button)
    {
        button.SetIconSprite(ability.icon);
    }
    
    public void Cancel()
    {
        menu.GoBack();
    }

    public void PickSlot(int i)
    {
        if (character.quickCastAbilities == null || character.quickCastAbilities.Length == 0)
        {
            character.quickCastAbilities = new AbilityBase[6];
        }

        character.quickCastAbilities[i] = ability;
        menu.GoBack();
    }

    public AbilityBase ability { get; set; }

}

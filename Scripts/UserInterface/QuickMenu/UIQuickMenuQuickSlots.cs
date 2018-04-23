using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickMenuQuickSlots : UIQuickMenuPage
{
    [SerializeField]
    private UISemiCircularSelectionController selectionController;

    [SerializeField]
    private UIIconButton[] abilityButtons;

    [SerializeField]
    private TMP_Text textAbilityLabel;

    private InputModule inputModule;

    private string debugString;

	// Use this for initialization
	void Start () {

        inputModule = avatarController.inputModule;

        selectionController.inputModule = avatarController.inputModule;

        for(int i = 0; i< abilities.Length; i++)
        {
            AbilityBase ability = abilities[i];

            if (ability != null)
            {

                abilityButtons[i].SetIconSprite(ability.icon);
                abilityButtons[i].onClick.AddListener(() => castAbility(ability));
            }

            abilityButtons[i].onSelect.AddListener(() => selectAbility(ability));
            abilityButtons[i].onDeSelect.AddListener(() => selectAbility(null));
        }
	}

    private void selectAbility(AbilityBase ability)
    {
        if (ability != null)
        {
            textAbilityLabel.text = ability.label;
        }
        else
        {
            textAbilityLabel.text = string.Empty;
        }
    }

    void Update()
    {
        debugString = inputModule.ToString() + "\n";

        if (inputModule.GetAxisUp(NeverdawnInputAxis.Trigger, NeverdawnInputAxisDirection.Positive))
        {
            menu.Close();
        }
    }

    public void SetLabel(string label)
    {
        textAbilityLabel.text = label;
    }
    
    public void Cancel()
    {
        menu.GoBack();
    }

    private void castAbility(AbilityBase ability)
    {
        avatarController.CastAbility(ability);
    }

    public AbilityBase[] abilities { get; set; }
}

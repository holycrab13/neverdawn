using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAbilityIconButton : UIIconButton
{
    public AbilityBase ability { get; set; }
    public UIQuickMenu menu { get; set; }
    public TMP_Text label { get; set; }

    protected override void Start()
    {
        base.Start();

        if (ability)
        {
            SetIconSprite(ability.icon);
            onClick.AddListener(() => castAbility(ability));
            onSelect.AddListener(() => setLabel(ability.label));
        }
    }

    private void setLabel(string p)
    {
        if (label)
        {
            label.text = p;
        }
    }

    private void castAbility(AbilityBase ability)
    {
        if (menu)
        {
            menu.avatarController.CastAbility(ability);
            UIQuickMenuCastAbility instance = Instantiate(UIFactory.uiQuickMenuCastAbilityPrefab);
            instance.ability = ability;

            menu.NavigateInto(instance);
        }
    }
}

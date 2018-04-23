using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICombatItemIconButton : UIIconButton
{
    public CombatItem combatItem { get; set; }
    public UIQuickMenu menu { get; set; }
    public TMP_Text label { get; set; }
    public string customLabel { get; set; }

    protected override void Start()
    {
        base.Start();

        if (combatItem)
        {
            SetIconSprite(combatItem.identity.icon);
            onClick.AddListener(() => selectCombatItem());
            onSelect.AddListener(() => setLabel(combatItem.identity.label));
        }
    }

    private void setLabel(string p)
    {
        if (label)
        {
            label.text = customLabel != null ? customLabel : p;
        }
    }

    private void selectCombatItem()
    {
        if (menu)
        {
            UIQuickMenuCombatItem instance = Instantiate(UIFactory.uiQuickMenuCombatItemPrefab);
            instance.combatItem = combatItem;
            menu.NavigateInto(instance);
        }
    }

}

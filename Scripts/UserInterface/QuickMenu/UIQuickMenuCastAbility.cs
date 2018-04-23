using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class UIQuickMenuCastAbility : UIQuickMenuPage
{
    public AbilityBase ability { get; set; }

    [SerializeField]
    private TMP_Text textLabel;

    [SerializeField]
    private Image imageIcon;

    void Start()
    {
        imageIcon.sprite = ability.icon;
        textLabel.text = "Pick a target for " + ability.label;
    }

    void Update()
    {
        if (!ability.cursor)
        {
            menu.GoBack();
        }
    }
}

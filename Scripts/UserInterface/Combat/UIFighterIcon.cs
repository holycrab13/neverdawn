using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFighterIcon : MonoBehaviour {

    public Image icon;

    public TMP_Text label;

    public UINumericBar healthBar;

    public UINumericBar manaBar;

    [SerializeField]
    private Image background;

    private Character character;
    private Destructible destructible;

    private int cachedHealthPoints;
    private int cachedMaxHealthPoints;
    private Caster caster;
    private int cachedManaPoints;
    private int cachedMaxManaPoints;

    public Character GetCharacter()
    {
        return character;
    }

    void Update()
    {
        if(!destructible)
        {
            return;
        }

        if(destructible.healthPoints != cachedHealthPoints)
        {
            healthBar.SetValues(destructible.healthPoints, destructible.maxHealthPoints);
            cachedHealthPoints = destructible.healthPoints;
        }

        if (destructible.maxHealthPoints != cachedMaxHealthPoints)
        {
            healthBar.SetValues(destructible.healthPoints, destructible.maxHealthPoints);
            cachedMaxHealthPoints = destructible.maxHealthPoints;
        }

        if(caster != null)
        {
            if (caster.manaPoints != cachedManaPoints)
            {
                manaBar.SetValues(caster.manaPoints, caster.maxManaPoints);
                cachedManaPoints = caster.manaPoints;
            }

            if (caster.maxManaPoints != cachedMaxManaPoints)
            {
                manaBar.SetValues(caster.manaPoints, caster.maxManaPoints);
                cachedMaxManaPoints = caster.maxManaPoints;
            }
        }
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
        this.destructible = character.GetComponent<Destructible>();
        this.caster = character.GetComponent<Caster>();

        icon.sprite = character.identity.icon;
        // label.text = interactable.label;

    }


    internal void SetColor(Color color)
    {
        background.color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISequenceIcon : MonoBehaviour {

    [SerializeField]
    private LayoutElement iconElement;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image glow;

    [SerializeField]
    private Image health;


    [SerializeField]
    private Image mana;

    [SerializeField]
    private Text label;

    [SerializeField]
    private float sizeDelta;

    [SerializeField]
    private float scaleSpeed = 1.0f;

    [SerializeField]
    private RectTransform manaPanel;

    public Character character { get; set; }

    private float scale;

    private float baseSize;
    private Caster caster;
  
	// Use this for initialization
	void Start () {

        icon.sprite = character.identity.icon;
        label.text = character.identity.label;
        health.fillAmount = (float)character.health / (float)character.maxHealth;

        caster = character.GetComponent<Caster>();

        if(caster == null)
        {
            manaPanel.gameObject.SetActive(false);
        }

        baseSize = iconElement.preferredWidth;
        glow.color = scale * Color.white;
	}
	
	// Update is called once per frame
	void Update () {

        health.fillAmount = (float)character.health / (float)character.maxHealth;

        if (caster != null)
        {
            mana.fillAmount = caster.manaPercentage;
        }

        if(GameController.instance.combatController.IsCharacterTurn(character))
        {
     
            if(scale < 1.0f)
            {
                scale = Mathf.Clamp01(scale + Time.deltaTime * scaleSpeed);

                glow.color = scale * Color.white;
                iconElement.preferredWidth = baseSize + scale * sizeDelta;
                iconElement.preferredHeight = baseSize + scale * sizeDelta;
            }

            if (scale == 1.0f)
                label.gameObject.SetActive(true);
        }
        else
        {
            label.gameObject.SetActive(false);

            if(scale > 0.0f)
            {
                scale = Mathf.Clamp01(scale - Time.deltaTime * scaleSpeed);

                glow.color = scale * Color.white;
                iconElement.preferredWidth = baseSize + scale * sizeDelta;
                iconElement.preferredHeight = baseSize + scale * sizeDelta;
            }
        }
	}
}

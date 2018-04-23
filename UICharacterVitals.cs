using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterVitals : MonoBehaviour {

    [SerializeField]
    private UINumericBar _healthBar;

    [SerializeField]
    private UINumericBar _manaBar;

    [SerializeField]
    private UINumericBlockBar _staminaBar;

    [SerializeField]
    private Character _character;

    private Destructible _destructible;

    private Caster _caster;

    private int _cachedMaxHealth;

    private int currentHp;
    private int _cachedHealth;
    private int _cachedMaxMana;
    private int _cachedMana;
    private int _cachedSteps = -1;
    private int _cachedTotalSteps;

    public Character character
    {
        set
        {
            _character = value;

            if (_character)
            {
                _caster = _character.caster;
                _destructible = _character.destructible;

                if (_caster == null)
                    _manaBar.gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        if (_character)
        {
            character = _character;
        }
    }

	
	// Update is called once per frame
	void Update () {
		
        if(_destructible)
        {
            if(_destructible.maxHealthPoints != _cachedMaxHealth || _destructible.healthPoints != _cachedHealth)
            {
                _healthBar.SetValues(_destructible.healthPoints, _destructible.maxHealthPoints);
                _cachedHealth = _destructible.healthPoints;
                _cachedMaxHealth = _destructible.maxHealthPoints;
            }
        }

        if (_character.remainingSteps != _cachedSteps || _character.totalSteps != _cachedTotalSteps)
        {
            _cachedSteps = _character.remainingSteps;
            _cachedTotalSteps = _character.totalSteps;
            _staminaBar.SetValues(_cachedSteps, _cachedTotalSteps);
        }

        if (_caster)
        {
            if (_caster.maxManaPoints != _cachedMaxMana || _caster.manaPoints != _cachedMana)
            {
                _manaBar.SetValues(_caster.manaPoints, _caster.maxManaPoints);
                _cachedMana = _caster.manaPoints;
                _cachedMaxMana = _caster.maxManaPoints;
            }
        }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject, IQuickMenuInteractionCollection
{
    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _label;

    [SerializeField]
    private string _description;

    [SerializeField]
    private int _priority;

    [SerializeField]
    private bool _usableOutOfCombat;

    [SerializeField]
    private CharacterSkillLevel[] _requirements;

    [SerializeField]
    private int _manaCost;

    [SerializeField]
    private int _actionCost;

    [SerializeField]
    private AnimationClip _animation;

    private AbilityCursorBase _cachedCursor;

    
    /// <summary>
    /// Cast the ability. This creates an action that can be pushed to a character
    /// </summary>
    /// <returns>The action</returns>
    public virtual CharacterActionBase Cast()
    {
        FloatingTextFactory.Create(_label, caster.position + Vector3.up, new Color(1.0f, 1.0f, 1.0f), 24);

        caster.mana -= manaCost;

        if (GameController.state == GameState.Combat)
            caster.remainingActions -= actionCost;

        return null;
    }

    public virtual CharacterActionBase Prepare()
    {
        return null;
    }

    public virtual CharacterActionBase Relax()
    {
        return null;
    }

    protected virtual AbilityCursorBase createCursor()
    {
        return null;
    }

    public AbilityCursorBase CreateCursorForController(AvatarController controller)
    {
        if(controller == null)
        {
            _cachedCursor = null;
            return null;
        }

        _cachedCursor = createCursor();
        _cachedCursor.Initialize(controller);
        return _cachedCursor;
    }

    public void DestroyCursor()
    {
        if (_cachedCursor)
        {
            Destroy(_cachedCursor.gameObject);
        }
    }

    public virtual void ApplyCursor()
    {

    }

    public virtual bool Initialize(GameObject gameObject)
    {
        return true;
    }

    protected virtual bool isCastable(Character character)
    {
        return true;
    }

    public bool IsCastable(Character character)
    {
        if (GameController.state != GameState.Combat && !usableOutOfCombat)
        {
            return false;
        }

        if (!CharacterHasSkill(character))
        {
            return false;
        }

        return isCastable(character);
    }

    public bool CharacterHasEnoughMana(Character character)
    {
        return character.mana >= manaCost;
    }

    public bool CharacterHasSkill(Character character)
    {
         if (requirements != null)
        {
            foreach (CharacterSkillLevel condition in requirements)
            {
                if (!character.HasSkill(condition))
                {
                    return false;
                }
            }
        }

        return true;
    }


    internal string GetRequirementString(Character character)
    {
        List<string> requires = new List<string>();

        foreach (CharacterSkillLevel condition in requirements)
        {
            if (!character.HasSkill(condition))
            {
                CharacterSkill skill = NeverdawnDatabase.GetSkill(condition.type);
                requires.Add(string.Format("{0} {1}", skill.label, condition.value));
            }
        }

        return "Requires: " + string.Join(", ", requires.ToArray());
    }

    public virtual Sprite icon
    {
        get
        {
            return _icon;
        }
    }

    

    public virtual string label { get { return _label; } }

    public virtual string description { get { return _description; } }

    public int priority { get { return _priority; } }

    public bool usableOutOfCombat { get { return _usableOutOfCombat; } }

    public CharacterSkillLevel[] requirements { get { return _requirements; } }

    public int manaCost { get { return _manaCost; } }

    public int actionCost { get { return _actionCost; } }

    public bool hasTarget { get; private set; }

    public AnimationClip animation { get { return _animation; } }

    public QuickMenuInteraction[] interactions
    {
        get { throw new NotImplementedException(); }
    }

    public AbilityCursorBase cursor
    {
        get { return _cachedCursor; }
    }

    public int revision
    {
        get { throw new NotImplementedException(); }
    }

    public Character caster { get; set; }

   
}

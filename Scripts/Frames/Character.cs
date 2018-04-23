using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


/// <summary>
/// A character in the game world. This class holds level, skills and attributes
/// </summary>
[RequireComponent(typeof(Identity))]
[RequireComponent(typeof(Destructible))]
public class Character : FrameComponent
{
    /// <summary>
    /// The action queue of the character
    /// </summary>
    private Queue<CharacterActionBase> _actions;

    /// <summary>
    /// All active buffs on the character
    /// </summary>
    private List<BuffBase> _activeBuffs;

    /// <summary>
    /// The remaining steps for a character when in combat mode
    /// </summary>
    private int _remainingSteps;

    /// <summary>
    /// The cached character transform
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// The animator in the character solid component (if exists)
    /// </summary>
    private Animator _animator;

    private AnimatorOverrideController _animatorOverrideController;

    /// <summary>
    /// The cached destructible of the character or null
    /// </summary>
    private Destructible _destructible;

    /// <summary>
    /// The cached identity of the character or null
    /// </summary>
    private Identity _identity;

    /// <summary>
    /// The cached solid component of the character or null
    /// </summary>
    private Solid _solid;

    /// <summary>
    /// The cached mannequin of the character or null
    /// </summary>
    private Mannequin _mannequin;

    /// <summary>
    /// The cached unarmed weapon of the character or null
    /// </summary>
    private CombatItem _unarmed;

    private NavMeshObstacle _obstacle;

    /// <summary>
    /// The cached skillset component of the character or null
    /// </summary>
    private Skillable _skillable;

    /// <summary>
    /// The controller currently controlling this character
    /// </summary>
    private NeverdawnCharacterController _controller;

    /// <summary>
    /// The cached caster component of the character or null
    /// </summary>
    private Caster _caster;

    public AbilityBase[] quickCastAbilities = new AbilityBase[6];

    public NeverdawnCharacterController controller
    {
        get { return _controller; }
        set { _controller = value; }
    }

    void Awake()
    {
        if(_activeBuffs == null)
        {
            _activeBuffs = new List<BuffBase>();
        }

        if (_actions == null)
        {
            _actions = new Queue<CharacterActionBase>();
        }

    }

    void Start()
    {
        _remainingSteps = totalSteps;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BuffBase buff in _activeBuffs)
        {
            buff.UpdateBuff(this, Time.deltaTime);
        }

        _activeBuffs.RemoveAll(b => b.IsDone);


        if (currentAction != null)
        {
            if (currentAction.IsDone)
            {
                currentAction = null;
            }
            else
            {
                currentAction.ActionUpdate(Time.deltaTime);
            }
        }


        if (currentAction == null)
        {
            if (_actions.Count > 0)
            {
                currentAction = _actions.Dequeue();
                currentAction.ActionStart(this);
            }
        }
    }

    void TriggerOnGrabCombatItem()
    {
        if (mannequin != null)
        {
            mannequin.ShowItemInHandsView(activeCombatItem);
        }
    }

    public void EnterCombatStance()
    {
        CombatItem first = null;

        if (mannequin != null)
        {
            first = mannequin.GetFirstEquippedCombatItem();
        }

        if (first == null)
        {
            first = unarmed;
        }

        frame.UpdateTile();

        PushAction(new CharacterNavigateToAction(currentTile.transform.position));
        PushAction(new CharacterDrawCombatItemAction(first));
    }

    public void UpdateTile()
    {
        frame.UpdateTile();
    }

    internal void LeaveCombatStance()
    {
        PushAction(new CharacterSheathCombatItemAction());
        _remainingSteps = totalSteps;
    }

    internal void PushAction(CharacterActionBase action)
    {
        if (action == null)
            return;

        if (_actions == null)
            _actions = new Queue<CharacterActionBase>();

        _actions.Enqueue(action);
    }

    internal void StartTurn()
    {
        _remainingSteps = totalSteps;
        remainingActions = 1;
    }

    public void Consume(Consumable consumable)
    {
        foreach (BuffBase buff in consumable.buffs)
        {
            _activeBuffs.Add(buff);
        }

        consumable.frame.destroyed = true;
        Destroy(consumable.gameObject);
    }

    public void AddBuffs(IEnumerable<BuffBase> buffs)
    {
        foreach (BuffBase buff in buffs)
        {
            _activeBuffs.Add(buff);
        }
    }

    public IEnumerable<T> GetCastableAbilities<T>() where T : AbilityBase
    {
        IEnumerable<AbilityBase> result = GetAllAbilities();
        return result.Where(a => a is T && a.IsCastable(this)).Select(a => a as T).ToArray();
    }

    public IEnumerable<AbilityBase> GetAllAbilities()
    {
        List<AbilityBase> allAbilities = new List<AbilityBase>();

        if (mannequin != null)
        {
            CombatItem[] combatItems = mannequin.GetEquippedCombatItems();

            allAbilities.AddRange(combatItems.SelectMany(c => c.abilities));

            
        }

        if (unarmed != null)
        {
            allAbilities.AddRange(unarmed.abilities);
        }

        return allAbilities;
    }

    public void LevelUp()
    {
        if (skillable)
        {
            skillable.LevelUp();
        }
    }

    internal int GetAttributeLevel(AttributeType attributeType, bool includeAttributeBonus = false)
    {
        int attributeLevel = skillable ? skillable[attributeType] : 0;

        if (includeAttributeBonus && mannequin != null)
        {
            attributeLevel += mannequin.GetAttributeBonus(attributeType);
            attributeLevel += GetFoodBonus(attributeType);
        }

        return attributeLevel;
    }

    internal int GetSkillLevel(SkillType skillType, bool includeSkillBonus = false)
    {
        int skillLevel = skillable ? skillable[skillType] : 0;

        if (includeSkillBonus && mannequin != null)
        {
            skillLevel += GetAttributeLevel(NeverdawnDatabase.GetBaseAttributeType(skillType));
            skillLevel += mannequin.GetSkillBonus(skillType);
            skillLevel += GetFoodBonus(skillType);
        }

        return skillLevel;
    }

    internal bool HasSkill(CharacterSkillLevel condition)
    {
        return (skillable ? skillable[condition.type] : 0) >= condition.value;
    }

    public int GetFoodBonus(SkillType characterSkillType)
    {
        return 0;
    }

    public int GetFoodBonus(AttributeType characterAttributeType)
    {
        return 0;
    }

    public float GetCombatScore()
    {
        return (float)health / (float)maxHealth;
    }

    #region Properties

    public int initiative
    {
        get { return GetAttributeLevel(AttributeType.Dexterity); }
    }

    //public NeverdawnCombatItems combatItems
    //{
    //    get { return _combatItems; }
    //}

    //public NeverdawnArmor equipment
    //{
    //    get { return _equipment; }
    //}

    public int remainingSteps
    {
        get
        {
            return _remainingSteps;
        }
        set
        {
            _remainingSteps = value;
        }
    }

    public Transform cachedTransform
    {
        get
        {
            if (_transform == null)
            {
                _transform = GetComponent<Transform>();
            }

            return _transform;
        }
    }

    public Vector3 position
    {
        get
        {
            return cachedTransform.position;
        }
        set
        {
            cachedTransform.position = value;
        }
    }

    public Vector3 eulerAngles
    {
        get
        {
            return cachedTransform.eulerAngles;
        }
        set
        {
            cachedTransform.eulerAngles = value;
        }
    }

    public Vector3 forward
    {
        get
        {
            return cachedTransform.forward;
        }
        set
        {
            cachedTransform.forward = value;
        }
    }

    //public bool isCharacterTurn
    //{
    //    get
    //    {
    //        return _isCharacterTurn;
    //    }
    //}

    public bool isAlive
    {
        get
        {
            return health > 0;
        }
    }

    public int totalSteps
    {
        get
        {
            return 2 + (int)(0.1f * (skillable ? skillable[SkillType.CombatManeuvering] : 0));
        }
    }

   

    public int this[AttributeType attribute]
    {
        get { return skillable ? skillable[attribute] : 0; }
    }

    public int this[SkillType skill]
    {
        get { return skillable ? skillable[skill] : 0; }
    }

    public Skillable skillable
    {
        get
        {
            if (_skillable == null)
            {
                _skillable = GetComponent<Skillable>();
            }

            return _skillable;
        }
    }

    public Mannequin mannequin
    {
        get
        {
            if (_mannequin == null)
            {
                _mannequin = GetComponent<Mannequin>();
            }

            return _mannequin;
        }
    }


    public Caster caster
    {
        get
        {
            if (_caster == null)
            {
                _caster = GetComponent<Caster>();
            }

            return _caster;
        }
    }

    public Destructible destructible
    {
        get
        {
            if (_destructible == null)
            {
                _destructible = GetComponent<Destructible>();
            }

            return _destructible;
        }
    }

    public Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>(true);
            }

            return _animator;
        }
    }

    public NavMeshObstacle obstacle
    {
        get
        {
            if (_obstacle == null)
            {
                _obstacle = GetComponentInChildren<NavMeshObstacle>(true);
            }

            return _obstacle;
        }
    }

    public Identity identity
    {
        get
        {
            if(_identity == null)
            {
                _identity = GetComponent<Identity>();
            }

            return _identity;
        }
    }


    public CombatItem unarmed
    {
        get
        {
            if (_unarmed == null)
            {
                _unarmed = GetComponent<CombatItem>();
            }

            return _unarmed;
        }
    }

    public Solid solid
    {
        get
        {
            if (_solid == null)
            {
                _solid = GetComponent<Solid>();
            }
                
            return _solid;
        }
    }

    public int mana
    {
        get
        {
            Caster caster = GetComponent<Caster>();

            if (caster != null)
                return caster.manaPoints;

            return 0;
        }
        set
        {
            Caster caster = GetComponent<Caster>();

            if (caster != null)
                caster.manaPoints = value;
        }
    }

    public int health
    {
        get
        {
            return destructible.healthPoints;
        }
        set
        {
            destructible.healthPoints = value;
        }
    }


    public int maxHealth
    {
        get
        {
            return destructible.maxHealthPoints;
        }
        set
        {
            destructible.maxHealthPoints = value;
        }
    }

    public int maxMana
    {
        get
        {
            Caster caster = GetComponent<Caster>();

            if (caster != null)
                return caster.maxManaPoints;

            return 0;
        }
        set
        {
            Caster caster = GetComponent<Caster>();

            if (caster != null)
                caster.maxManaPoints = value;
        }
    }

    public bool isIdle
    {
        get
        {
            return currentAction == null && _actions.Count == 0;
        }
    }

    public CharacterActionBase currentAction { get; private set; }

    public AnimatorOverrideController animatorOverrideController
    {
        get
        {
            if (_animatorOverrideController == null)
            {
                if (animator.runtimeAnimatorController is AnimatorOverrideController)
                {
                    _animatorOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                }
                else
                {
                    _animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
                    animator.runtimeAnimatorController = _animatorOverrideController;
                }
            }

            return _animatorOverrideController;
        }
    }

    public CombatItem activeCombatItem { get; set; }

    public AbilityBase activeAbility { get; set; }
    
    /// <summary>
    /// Object in player party or in the open world
    /// </summary>
    public bool IsInPlayerParty
    {
        get { return GameController.instance.party.Contains(this); }
    }

    public int remainingActions { get; set; }

    #endregion

    internal void Kill()
    {
        if (animator)
        {
            animator.SetTrigger("Die");
        }

        if(obstacle)
        {
            obstacle.enabled = false;
        }

        Fighter fighter = GetComponent<Fighter>();
        if (fighter)
        {
            fighter.enabled = false;
        }
        
    }

    public HexTile currentTile
    {
        get { return frame.currentTile; }
        set { frame.currentTile = value; }
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[Serializable]
public struct CharacterSkillLevel
{
    public SkillType type;
    public int value;
}

[Serializable]
public struct CharacterAttributeLevel
{
    public AttributeType type;
    public int value;
}

/// <summary>
/// This class holds level, skills and attributes of an object in the game world
/// </summary>
public class Skillable : FrameComponent
{
    [SerializeField]
    private int _learningPoints;

    [SerializeField]
    private int _level;

    [SerializeField]
    private CharacterSkillLevel[] _baseSkills;

    [SerializeField]
    private CharacterAttributeLevel[] _baseAttributes;

    private int[] _skills;

    private int[] _attributes;

    /// <summary>
    /// Translates the skill and attribute bonuses in the editor to int arrays for serialization
    /// </summary>
    void Awake()
    {
        if (_skills == null)
        {
            _skills = new int[Enum.GetValues(typeof(SkillType)).Length];

            if (_baseSkills != null)
            {
                foreach (CharacterSkillLevel level in _baseSkills)
                {
                    _skills[(int)level.type] = level.value;
                }
            }
        }

        if (_attributes == null)
        {
            _attributes = new int[Enum.GetValues(typeof(AttributeType)).Length];

            if (_baseAttributes != null)
            {
                foreach (CharacterAttributeLevel level in _baseAttributes)
                {
                    _attributes[(int)level.type] = level.value;
                }
            }
        }
    }

    public int this[AttributeType attribute]
    {
        get { return _attributes[(int)attribute]; }
    }

    public int this[SkillType skill]
    {
        get { return _skills[(int)skill]; }
    }
    
    public void ImproveAttribute(AttributeType attributeType)
    {
        int level = _attributes[(int)attributeType];
        int cost = NeverdawnDatabase.GetUpgradeCost(attributeType, level);

        if (learningPoints >= cost)
        {
            learningPoints -= cost;
            _attributes[(int)attributeType]++;
        }
    }

    internal void ImproveSkill(SkillType skillType)
    {
        int level = _skills[(int)skillType];
        int cost = NeverdawnDatabase.GetUpgradeCost(skillType, level);

        if (learningPoints >= cost)
        {
            learningPoints -= cost;
            _skills[(int)skillType]++;
        }
    }

    public void LevelUp()
    {
        level++;
        learningPoints += 10;
    }

    internal bool HasSkill(CharacterSkillLevel condition)
    {
        return this[condition.type] >= condition.value;
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteInt(learningPoints);
        writer.WriteInt(level);
        writer.WriteIntArray(_skills);
    }

    protected override void readData(IMessageReader reader)
    {
        _learningPoints = reader.ReadInt();
        _level = reader.ReadInt();
        _skills = reader.ReadIntArray();
    }


    #region Properties

    public int learningPoints
    {
        get
        {
            return _learningPoints;
        }
        set
        {
            _learningPoints = value;
        }
    }

    public int level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
        }
    }


    #endregion

   
}

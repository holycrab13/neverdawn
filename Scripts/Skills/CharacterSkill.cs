using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[CreateAssetMenu(fileName = "New Skill", menuName = "Neverdawn/Character/Skill", order = 1)]
public class CharacterSkill : ScriptableObject
{

    [SerializeField]
    private string _label;

    [SerializeField]
    private string _description;

    [SerializeField]
    private AttributeType _baseAttribute;

     [SerializeField]
    private SkillType _type;

    public string label
    {
        get { return _label; }
    }

    public string description
    {
        get { return _description; }
    }

    public AttributeType baseAttribute
    {
        get { return _baseAttribute; }
    }

    public SkillType type
    {
        get { return _type; }
    }
}



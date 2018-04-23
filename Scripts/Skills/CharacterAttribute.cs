using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[CreateAssetMenu(fileName = "New Attribute", menuName = "Neverdawn/Character/Attribute", order = 1)]
public class CharacterAttribute : ScriptableObject
{
    [SerializeField]
    private string _label;

    [SerializeField]
    private string _description;

    [SerializeField]
    private Color _color;

    [SerializeField]
    private AttributeType _type;


    public AttributeType type
    { 
        get { return _type; } 
    }

    public string label
    {
        get { return _label; }
    }

    public string description
    {
        get { return _description; }
    }

    public Color color
    {
        get { return _color; }
    }
}

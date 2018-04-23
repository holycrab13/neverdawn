using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType
{
    Helmet,
    Torso,
    Gloves,
    Boots, 
}

[RequireComponent(typeof(Pickable))]
public class Armor : Equipable {

    [SerializeField]
    private ArmorType _type;

    [SerializeField]
    private int _armorValue;

    [SerializeField]
    private Mesh _armorMesh;

    [SerializeField]
    private Material _armorMaterial;

    private Identity _identity;


    internal int GetSkillBonus(SkillType characterSkillType)
    {
        SkillEnhancement skillEnhancement = GetComponent<SkillEnhancement>();

        if (skillEnhancement)
        {
            return skillEnhancement.GetSkillBonus(characterSkillType);
        }

        return 0;
    }

    internal int GetAttributeBonus(AttributeType characterAttributeType)
    {
        SkillEnhancement skillEnhancement = GetComponent<SkillEnhancement>();

        if (skillEnhancement)
        {
            return skillEnhancement.GetAttributeBonus(characterAttributeType);
        }

        return 0;
    }

    public ArmorType type
    {
        get { return _type; }
    }

    public int armorValue
    {
        get { return _armorValue; }
    }

    public Mesh armorMesh
    {
        get { return _armorMesh; }
    }

    public Material armorMaterial
    {
        get { return _armorMaterial; }
    }

    public Identity identity
    {
        get
        {
            if (_identity == null)
            {
                _identity = GetComponent<Identity>();
            }

            return _identity;
        }
    }
}

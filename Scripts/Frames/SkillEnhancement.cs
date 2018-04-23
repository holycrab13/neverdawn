using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillEnhancement : FrameComponent
{
    [SerializeField]
    private List<CharacterSkillLevel> _skillBonus;

    [SerializeField]
    private List<CharacterAttributeLevel> _attributeBonus;

    private Dictionary<SkillType, int> skillBonusMap;

    private Dictionary<AttributeType, int> attributeBonusMap;

    void Start()
    {
        skillBonusMap = new Dictionary<SkillType, int>();
        attributeBonusMap = new Dictionary<AttributeType, int>();

        _skillBonus.ForEach(s => skillBonusMap.Add(s.type, s.value));
        _attributeBonus.ForEach(a => attributeBonusMap.Add(a.type, a.value));
    }

    internal int GetSkillBonus(SkillType characterSkillType)
    {
        if (skillBonusMap.ContainsKey(characterSkillType))
        {
            return skillBonusMap[characterSkillType];
        }

        return 0;
    }

    internal int GetAttributeBonus(AttributeType characterAttributeType)
    {
        if (attributeBonusMap.ContainsKey(characterAttributeType))
        {
            return attributeBonusMap[characterAttributeType];
        }

        return 0;
    }
}

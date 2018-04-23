using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickMenuCharacterStats : UIQuickMenuPage {

    [SerializeField]
    private UISkillView skillViewPrefab;

    [SerializeField]
    private RectTransform skillViewParent;

    [SerializeField]
    private UIQuickMenuSkillUp skillUpPrefab;

    private int selectedIndex;
    private List<UIButton> buttons;

    [SerializeField]
    private UIListSelectionController listController;

    private Dictionary<AttributeType, UISkillView> attributeViewMap;

    private Dictionary<SkillType, UISkillView> skillViewMap;

	// Use this for initialization
	void Start () {

        buttons = new List<UIButton>();
        attributeViewMap = new Dictionary<AttributeType, UISkillView>();
        skillViewMap = new Dictionary<SkillType, UISkillView>();

        Character character = avatarController.character;

        listController.inputModule = avatarController.inputModule;

        int k = 0;

        foreach (CharacterAttribute attribute in NeverdawnDatabase.attributes)
        {
            if (attribute != null)
            {
                UISkillView attributeView = Instantiate(skillViewPrefab);
                int i = k++;

                attributeView.transform.SetParent(skillViewParent);
                attributeView.totalValue = string.Empty;
                attributeView.label = attribute.label;
                attributeView.value = character.GetAttributeLevel(attribute.type, true).ToString();
                attributeView.labelColor = attribute.color;

                attributeView.button.onClick.AddListener(() => attributeSelected(attribute, i));
                attributeViewMap.Add(attribute.type, attributeView);

                buttons.Add(attributeView.button);
            }
        }

        GameObject go = new GameObject("gap");
        go.AddComponent<RectTransform>();
        LayoutElement layout = go.AddComponent<LayoutElement>();
        layout.preferredHeight = 10;

        go.transform.SetParent(skillViewParent);

        foreach (CharacterSkill skill in NeverdawnDatabase.skills)
        {
            if (skill != null)
            {
                int i = k++;

                CharacterAttribute baseAttribute = NeverdawnDatabase.GetAttribute(skill.baseAttribute);

                UISkillView skillView = Instantiate(skillViewPrefab);
                skillView.transform.SetParent(skillViewParent);
                skillView.label = skill.label;
                skillView.totalValue = string.Empty;
                skillView.value = character.GetSkillLevel(skill.type, true).ToString();
                skillView.totalValueColor = baseAttribute.color;
                skillView.button.onClick.AddListener(() => skillSelected(skill, i));
                skillViewMap.Add(skill.type, skillView);

                buttons.Add(skillView.button);
            }
        }

        selectedIndex = 0;
	}

    private void skillSelected(CharacterSkill skill, int selectedIndex)
    {
        CharacterAttribute attribute = NeverdawnDatabase.GetAttribute(skill.baseAttribute);

        UIQuickMenuSkillUp skillUp = Instantiate(skillUpPrefab);

        int level = avatarController.character.GetSkillLevel(skill.type);

        skillUp.label = skill.label;
        skillUp.description = skill.description;
        skillUp.baseValue = level;
        skillUp.attributeValue = avatarController.character.GetAttributeLevel(attribute.type);
        skillUp.equipmentValue = avatarController.character.mannequin ? avatarController.character.mannequin.GetSkillBonus(skill.type) : 0;
        skillUp.foodValue = avatarController.character.GetFoodBonus(skill.type);
        skillUp.cost = NeverdawnDatabase.GetUpgradeCost(skill.type, level);
        skillUp.learningPoints = avatarController.character.skillable.learningPoints;
        skillUp.showAttributes = true;
        skillUp.attributeColor = attribute.color;
        skillUp.attributeLabel = attribute.label;

        skillUp.confirm.interactable = avatarController.character.skillable.learningPoints >= NeverdawnDatabase.GetUpgradeCost(skill.type, level);
        skillUp.confirm.onClick.AddListener(() => skillUpSkill(skillUp, skill.type));

        this.selectedIndex = selectedIndex;
        menu.NavigateInto(skillUp);
    }

    private void skillUpSkill(UIQuickMenuSkillUp sender, SkillType characterSkillType)
    {
        int level = avatarController.character.GetSkillLevel(characterSkillType);
        int cost = NeverdawnDatabase.GetUpgradeCost(characterSkillType, level);

        avatarController.character.skillable.ImproveSkill(characterSkillType);

        int newCost = NeverdawnDatabase.GetUpgradeCost(characterSkillType, level + 1);

        sender.confirm.interactable = avatarController.character.skillable.learningPoints >= newCost;
        sender.baseValue = level + 1;
        sender.cost = newCost;
        sender.learningPoints = avatarController.character.skillable.learningPoints;
    }

    private void attributeSelected(CharacterAttribute attribute, int selectedIndex)
    {
        int level = avatarController.character.GetAttributeLevel(attribute.type);

        UIQuickMenuSkillUp skillUp = Instantiate(skillUpPrefab);
        skillUp.label = attribute.label;
        skillUp.description = attribute.description;
        skillUp.baseValue = level;
        skillUp.equipmentValue = avatarController.character.mannequin ? avatarController.character.mannequin.GetAttributeBonus(attribute.type) : 0;
        skillUp.foodValue = avatarController.character.GetFoodBonus(attribute.type);
        skillUp.attributeValue = 0;
        skillUp.cost = NeverdawnDatabase.GetUpgradeCost(attribute.type, level);
        skillUp.learningPoints = avatarController.character.skillable.learningPoints;
        skillUp.showAttributes = false;
        skillUp.baseColor = attribute.color;
        //skillUp.currentLabel = "Current level";
        //skillUp.currentValue = avatarController.character.GetAttributeLevel(attribute.type).ToString();
        //skillUp.pointsLabel = "Remaining attribute points";
        //skillUp.pointsValue = avatarController.character.learningPoints.ToString();
        //skillUp.color = attribute.color;

        skillUp.confirm.interactable = avatarController.character.skillable.learningPoints >= NeverdawnDatabase.GetUpgradeCost(attribute.type, level);
        skillUp.confirm.onClick.AddListener(() => skillUpAttribute(skillUp, attribute.type));

        this.selectedIndex = selectedIndex;
        menu.NavigateInto(skillUp);
    }

    private void skillUpAttribute(UIQuickMenuSkillUp sender, AttributeType attributeType)
    {
        int level = avatarController.character.GetAttributeLevel(attributeType);
        int cost = NeverdawnDatabase.GetUpgradeCost(attributeType, level);

        avatarController.character.skillable.ImproveAttribute(attributeType);

        int newCost = NeverdawnDatabase.GetUpgradeCost(attributeType, level + 1);

        sender.confirm.interactable = avatarController.character.skillable.learningPoints >= newCost;
        sender.baseValue = level + 1;
        sender.cost = newCost;
        sender.learningPoints = avatarController.character.skillable.learningPoints;
    }

    protected override void OnQuickMenuPageEnabled()
    {
        Character character = avatarController.character;

        selectedIndex = NeverdawnUtility.RepeatIndex(selectedIndex, buttons.Count);

        foreach(KeyValuePair<SkillType, UISkillView> entry in skillViewMap)
        {
            entry.Value.value = character.GetSkillLevel(entry.Key, true).ToString();
        }

        foreach (KeyValuePair<AttributeType, UISkillView> entry in attributeViewMap)
        {
            entry.Value.value = character.GetAttributeLevel(entry.Key, true).ToString();
        }

        buttons[selectedIndex].Select();
    }
}

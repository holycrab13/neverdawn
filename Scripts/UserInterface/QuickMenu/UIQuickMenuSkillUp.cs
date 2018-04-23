using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuickMenuSkillUp : UIQuickMenuPage {

    [SerializeField]
    private Text textLabel;

    [SerializeField]
    private Text textTotalValue;

    [SerializeField]
    private Text textDescription;

    [SerializeField]
    private Text textBaseValue;

    [SerializeField]
    private Text textBaseLabel;

    [SerializeField]
    private Text textAttributeValue;

    [SerializeField]
    private Text textAttributeLabel;

    [SerializeField]
    private Text textEquipmentValue;

    [SerializeField]
    private Text textFoodValue;

    [SerializeField]
    private Text textCostValue;

    [SerializeField]
    private Text textLearningPointsValue;

    [SerializeField]
    private Image imageBase;

    [SerializeField]
    private Image imageAttributes;

    [SerializeField]
    private Image imageEquipment;

    [SerializeField]
    private Image imageFood;

    [SerializeField]
    private RectTransform panelAttributes;

    private int _baseValue;

    private int _attributeValue;

    private int _equipmentValue;

    private int _foodValue;

    private int _cost;

    private int _learningPoints;

    public int baseValue
    {
        get
        {
            return _baseValue;
        }
        set
        {
            _baseValue = value;
            textBaseValue.text = _baseValue.ToString();
            updateWheel();
        }
    }

    public int attributeValue
    {
        get
        {
            return _attributeValue;
        }
        set
        {
            _attributeValue = value;
            textAttributeValue.text = _attributeValue.ToString();
            updateWheel();
        }
    }

    public int equipmentValue
    {
        get
        {
            return _equipmentValue;
        }
        set
        {
            _equipmentValue = value;
            textEquipmentValue.text = _equipmentValue.ToString();
            updateWheel();
        }
    }

    public int foodValue
    {
        get
        {
            return _foodValue;
        }
        set
        {
            _foodValue = value;
            textFoodValue.text = _foodValue.ToString();
            updateWheel();
        }
    }

    public int cost
    {
        get 
        { 
            return _cost; 
        }
        set
        {
            _cost = value;
            textCostValue.text = _cost.ToString();
        }
    }

    public int learningPoints
    {
        get
        {
            return _learningPoints;
        }
        set
        {
            _learningPoints = value;
            textLearningPointsValue.text = string.Format("/ {0}", _learningPoints);
        }
    }

    public string description
    {
        set
        {
            textDescription.text = value;
        }
    }

    public string label
    {
        set
        {
            textLabel.text = value;
        }
    }
    
    public string attributeLabel
    {
        set
        {
            textAttributeLabel.text = value;
        }
    }

    private void updateWheel()
    {
        float totalValue = (float)(foodValue + equipmentValue + attributeValue + baseValue);

        float foodAmount = (float)foodValue / totalValue;
        float baseAmount = (float)baseValue / totalValue;
        float equipmentAmount = (float)equipmentValue / totalValue;
        float attributeAmount = (float)attributeValue / totalValue;

        imageFood.fillAmount = 1.0f;
        imageEquipment.fillAmount = 1.0f - foodAmount;
        imageAttributes.fillAmount = 1.0f - (foodAmount + equipmentAmount);
        imageBase.fillAmount = 1.0f - (foodAmount + equipmentAmount + attributeAmount);
        textTotalValue.text = ((int)totalValue).ToString();
    }

    [SerializeField]
    private UIQuickMenuIconButton cancelButton;

    [SerializeField]
    private UIQuickMenuIconButton confirmButton;

    public Color attributeColor
    {
        set
        {
            textAttributeValue.color = value;
            imageAttributes.color = value;
            textAttributeLabel.color = value;
        }
    }

    public Color baseColor
    {
        set
        {
            textBaseLabel.color = value;
            textBaseValue.color = value;
            imageBase.color = value;
        }
    }
   
    public UIIconButton confirm
    {
        get { return confirmButton; }
    }

	// Use this for initialization
	void Start () {

        cancelButton.onClick.AddListener(cancel);
        cancelButton.Select();
	}

    private void cancel()
    {
        menu.GoBack();
    }


    public bool showAttributes
    {
        set
        {
            panelAttributes.gameObject.SetActive(value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillView : MonoBehaviour {

    [SerializeField]
    private Text labelText;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private Text totalText;

    [SerializeField]
    private Image image;

    public string label
    {
        set { labelText.text = value; }
    }

    public Color labelColor
    {
        set { labelText.color = value; }
    }

    public string value
    {
        set { valueText.text = value.ToString(); }
    }

    public string totalValue
    {
        set { totalText.text = value; }
    }

    public Color totalValueColor
    {
        set
        {
            totalText.color = value;
        }
    }

    public void SetValue(string label, int value, Color backgroundColor)
    {
        backgroundColor.a = 0.5f;

        labelText.text = label;
        labelText.color = backgroundColor;
        valueText.text = value.ToString();
        totalText.text = string.Empty;
    }

    public void SetValue(string label, int value, int total, Color totalColor)
    {
        labelText.text = label;
        valueText.text = string.Format("({0})", value.ToString());
        totalText.text = total.ToString();
        totalText.color = totalColor;
    }

    public UIButton button
    {
        get { return GetComponent<UIButton>(); }
    }

}

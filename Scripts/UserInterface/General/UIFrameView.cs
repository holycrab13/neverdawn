using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFrameView : MonoBehaviour {

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Text labelText;

    [SerializeField]
    private Text descriptionText;

    [SerializeField]
    private Sprite defaultIcon;

    [SerializeField]
    private string _defaultLabel;

    [SerializeField]
    private string defaultDescription;

    public string defaultLabel
    {
        get { return _defaultLabel; }
        set { _defaultLabel = value; }
    }

    public FrameComponent component
    {
        set
        {
            if (value != null)
            {
                SetFrame(value.GetComponent<Identity>());
            }
            else
            {
                Reset();
            }
        }
    }
    

    private void Reset()
    {
        if (iconImage != null)
        {
            if (defaultIcon != null)
            {
                iconImage.sprite = defaultIcon;
            }
            else
            {
                iconImage.enabled = false;
            }
        }

        if (labelText != null)
            labelText.text = _defaultLabel;

        if (descriptionText != null)
            descriptionText.text = defaultDescription;
    }

    public void SetFrame(Identity identity)
    {
        if (identity != null)
        {
            if (iconImage != null)
            {
                if (identity.icon != null)
                {
                    iconImage.enabled = true;
                    iconImage.sprite = identity.icon;
                }
                else
                {
                    if (defaultIcon != null)
                    {
                        iconImage.sprite = defaultIcon;
                    }
                    else
                    {
                        iconImage.enabled = false;
                    }
                }
            }

            if (labelText != null)
                labelText.text = identity.label;

            if (descriptionText != null)
                descriptionText.text = identity.description;
        }
        else
        {
            Reset();
        }
    }
}

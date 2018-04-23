using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextMessage : MonoBehaviour {

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text text;

    public string message
    {
        set
        {
            text.text = value;
        }
    }

    public Sprite icon
    {
        set
        {
            image.sprite = value;
        }
    }
}

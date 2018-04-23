using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChatPanel : MonoBehaviour {

    [SerializeField]
    private UITextReveal text;

    [SerializeField]
    private Image image;

    public Sprite icon
    {
        set { image.sprite = value; }
    }

    public string message
    {
        set { text.Reveal(value); }
    }

}

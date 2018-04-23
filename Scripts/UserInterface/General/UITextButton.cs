using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextButton : UIButton
{
    [SerializeField]
    private Text label;

    [SerializeField]
    private TMP_Text textMesh;

    public string text
    {
        get { return label.text; }
        set 
        { 
            if(label != null)
                label.text = value;

            if(textMesh != null)
                textMesh.text = value;
        }
    }

}

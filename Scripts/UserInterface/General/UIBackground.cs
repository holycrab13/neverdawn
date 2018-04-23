using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackground : MonoBehaviour
{

    [SerializeField]
    private Image backgroundImage;

    void Start()
    {
        backgroundImage.color = UISettings.backgroundColor;
    }
}

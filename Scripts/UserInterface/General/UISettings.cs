using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    public static Color backgroundColor { get; private set; }

    [SerializeField]
    private Color _backgroundColor;

    void Awake()
    {
        backgroundColor = _backgroundColor;
    }
}

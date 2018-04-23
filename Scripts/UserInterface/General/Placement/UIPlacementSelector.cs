using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlacementSelector : MonoBehaviour
{
    public Color color { get; set; }

    void Start()
    {
        GetComponent<Image>().color = color;
    }
}

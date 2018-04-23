using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINumericBar : MonoBehaviour {

    public Image fill;

    public TMP_Text label;

    internal void SetValues(int p1, int p2)
    {
        label.text = string.Format("{0} / {1}", p1, p2);
        fill.fillAmount = (float)p1 / (float)p2;
    }
}

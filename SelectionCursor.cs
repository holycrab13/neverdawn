using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCursor : MonoBehaviour {

    public Color color
    {
        set
        {
            GetComponent<SpriteRenderer>().color = value;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("FrameComponent/Identity")]
public class Identity : FrameComponent {

    [SerializeField]
    private string _label;

    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _description;

    public string label
    {
        get { return _label; }
        set { _label = value; }
    }

    public string description
    {
        get { return _description; }
        private set { _description = value; }
    }

    public Sprite icon
    {
        get { return _icon; }
    }
}

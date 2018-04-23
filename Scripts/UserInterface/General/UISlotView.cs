using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UISelectable))]
public class UISlotView : UIFrameView {

    [SerializeField]
    private TMP_Text labelSlotName;

    private UISelectable _selectable;

    public UISelectable selectable
    {
        get
        {
            if (_selectable == null)
                _selectable = GetComponent<UISelectable>();

            return _selectable;
        }
    }

    public string slotName
    {
        set { if(labelSlotName != null) labelSlotName.text = value; }
    }
}

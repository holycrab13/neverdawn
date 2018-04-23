using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatItemSlotView : MonoBehaviour {

    [SerializeField]
    private Transform viewLeftHand;

    [SerializeField]
    private Transform viewRightHand;

    [SerializeField]
    private CombatItemSlotType _type;

    public CombatItemSlotType type
    {
        get { return _type; }
    }

    private bool empty;


    void Start()
    {
        empty = true;
    }

    public void SetCombatItem(CombatItem item)
    {
        empty = true;

        GameObject drawableRightHand = item != null ? item.drawableRightHand : null;
        GameObject drawableLeftHand = item != null ? item.drawableLeftHand : null;

        if(viewLeftHand != null)
        {
            updateView(viewLeftHand, drawableLeftHand);
        }

        if (viewRightHand != null)
        {
            updateView(viewRightHand, drawableRightHand);
        }
    }

    private void updateView(Transform view, GameObject drawable)
    {
        foreach (Transform child in view)
        {
            Destroy(child.gameObject);
        }

        if(drawable != null)
        {
            GameObject instance = Instantiate(drawable);
            instance.transform.SetParent(view, false);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            empty = false;
        }

    }

    public bool isEmpty
    {
        get
        {
            return empty;
        }
    }
}

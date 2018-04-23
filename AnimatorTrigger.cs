using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour {


    void OnGrabCombatItem()
    {
        SendMessageUpwards("TriggerOnGrabCombatItem", SendMessageOptions.DontRequireReceiver);
    }
}

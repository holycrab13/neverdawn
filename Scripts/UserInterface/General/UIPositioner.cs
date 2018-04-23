using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositioner : MonoBehaviour {

    private AvatarController controller;


    void OnEnable()
    {
        controller = GetComponent<UIQuickMenu>().avatarController;

        if (controller != null && controller.character != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(controller.character.position + 1.5f * Vector3.up);
            transform.position = screenPos;
        }
    }

    void Update()
    {
        if (controller.character != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(controller.character.position + 1.5f * Vector3.up);
            transform.position = screenPos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISharedPage : UIPage {

    private UIInputController inputController;

    public override void OnShow()
    {
        inputController = GetComponentInParent<UIInputController>();
        inputController.SetInputModules(DeviceController.inputModules);
        base.OnShow();
    }

    public override void OnControllerActivated(InputModule module)
    {
        inputController.SetInputModules(DeviceController.inputModules);
    }
}

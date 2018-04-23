using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPauseMenu : UIPage {

    [SerializeField]
    private UISelectable firstButton;

    [SerializeField]
    private UIInputController controller;

    [SerializeField]
    private UIPage menuMainPage;

    public override void OnShow()
    {
        controller.SetInputModules(DeviceController.inputModules);
        base.OnShow();
    }

    public void Continue()
    {
        GameController.instance.UnPauseGame();
    }

    public void SaveGame()
    {
        GameController.instance.Save("Test");
    }

    public void GoToMainMenu()
    {
        GameController.instance.GoToMainMenu();
    }

    
}

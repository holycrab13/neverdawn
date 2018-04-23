using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIMainMenu : MonoBehaviour {

    private UIInputController inputController;

    private UIPageGroup group;
    
    public void OnShow()
    {
        inputController = GetComponent<UIInputController>();
        group = GetComponent<UIPageGroup>();

        if (DeviceController.inputModules.Length > 0)
        {
            inputController.SetInputModules(DeviceController.inputModules);
            group.ShowPage(1);
        }
        else
        {
            group.ShowPage(0);
        }
    }

 

    public void Quit()
    {
        Application.Quit();
    }
}

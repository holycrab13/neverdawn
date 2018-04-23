using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour {

    private static UIRoot instance;

    [SerializeField]
    private UIPage activePage;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        foreach (UIPage page in GetComponentsInChildren<UIPage>())
        {
            page.OnHide();
        }

        activePage.OnShow();

        DeviceController.onModuleActivated += controllerActivated;

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void controllerActivated(InputModule module)
    {
        if (activePage != null)
        {
            activePage.OnControllerActivated(module);
        }
    }

    public static bool exists
    {
        get { return FindObjectOfType<UIRoot>() != null; }
    }

    public void ShowPage(UIPage page)
    {
        if(activePage != null)
        {
            activePage.OnHide();
        }

        activePage = page;

        if (activePage != null)
        {
            activePage.OnShow();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartPage : UIPage {

    [SerializeField]
    private UIPage mainPage;

    public override void OnControllerActivated(InputModule module)
    {
        mainPage.Show();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterMenu : MonoBehaviour {

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image background;


    [SerializeField]
    private UICharacterVitals vitals;

    private NeverdawnCharacterController _chachedController;

    private Character _character;

    [SerializeField]
    private Color _defaultBackgroundColor;

    [SerializeField]
    private UIQuickMenu _quickMenu;

    public UIQuickMenuPage currentPage
    {
        get { return _quickMenu.currentPage; }
    }

    

    internal void Close()
    {
        _quickMenu.Close();
    }

	// Update is called once per frame
	void Update () {

        if (_chachedController != _character.controller)
        {
            if(_character.controller) 
            {
                background.color = _character.controller.color;
            }
            else
            {
                background.color = _defaultBackgroundColor;
            }

            _chachedController = _character.controller;
        }
      
	}

    public Character character
    {
        get
        {
            return _character;
        }
        set
        {
            _character = value;
            icon.sprite = _character.identity.icon;
            vitals.character = _character;
            GetComponent<UIQuickMenu>().character = _character;
        }
    }


    internal void Open(UIQuickMenuPage page)
    {
        _quickMenu.Open(page);
    }

    internal void NavigateInto(UIQuickMenuPage page)
    {
        _quickMenu.NavigateInto(page);
    }

    public bool isOpen
    {
        get { return _quickMenu.isOpen; }
    }

    internal void GoBack()
    {
        _quickMenu.GoBack();
    }
}

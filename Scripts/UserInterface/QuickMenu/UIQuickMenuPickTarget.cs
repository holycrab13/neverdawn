using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMenuPickTarget : UIQuickMenuPage {

    
    [SerializeField]
    private UIIconButton iconButtonPrefab;

    [SerializeField]
    private RectTransform iconButtonParent;

    [SerializeField]
    private UICircularSelectionController selectionController;

    [SerializeField]
    private UIIconButton cancelButton;

    public Character[] possibleTargets { get; set; }

    public SingleTargetCursor cursor { get; set; }

    private Dictionary<UISelectable, Character> buttonMap;

    public Character selected
    {
        get
        {
            UISelectable button = selectionController.selected;

            if(button != null && buttonMap.ContainsKey(button))
            {
                return buttonMap[button];
            }

            return null;
        }
    }

	// Use this for initialization
	void Start () {

        List<UIIconButton> buttons = new List<UIIconButton>();
        buttonMap = new Dictionary<UISelectable, Character>();

	    foreach(Character character in possibleTargets)
        {
            UIIconButton iconButton = Instantiate(iconButtonPrefab);
            iconButton.SetIconSprite(character.identity.icon);
            iconButton.transform.SetParent(iconButtonParent);

            buttonMap.Add(iconButton, character);

            buttons.Add(iconButton);
        }

        selectionController.peripherals = buttons.ToArray();
        selectionController.center = cancelButton;
        selectionController.inputModule = avatarController.inputModule;
	}

}

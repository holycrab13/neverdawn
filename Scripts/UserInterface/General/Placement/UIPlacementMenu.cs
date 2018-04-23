//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UIPlacementMenu : MonoBehaviour {

//    private static UIPlacementMenu instance;

//    [SerializeField]
//    private UIPlacementButton buttonPrefab;

//    [SerializeField]
//    private RectTransform buttonParent;

//    private List<AvatarController> controllers;

//    private List<UIPlacementButton> buttons;

//    private int length;

//    private List<Character> charactersToPlace;

//    private AvatarController confirmedController;

//    void Start()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }

//        instance.controllers = new List<AvatarController>();
//        buttons = new List<UIPlacementButton>();

//        gameObject.SetActive(false);
//    }

//    void Update()
//    {
//        foreach(AvatarController controller in controllers)
//        {
//            InputModule input = controller.inputModule;

//            if(input.GetButtonDown(NeverdawnInputButton.Left))
//            {
//                controller.placementSelectionIndex = NeverdawnUtility.RepeatIndex(controller.placementSelectionIndex - 1, length);
//            }

//            if (input.GetButtonDown(NeverdawnInputButton.Right))
//            {
//                controller.placementSelectionIndex = NeverdawnUtility.RepeatIndex(controller.placementSelectionIndex + 1, length);
//            }

//            if(input.GetButtonDown(NeverdawnInputButton.Confirm) && controller.character != null)
//            {
//                confirmedController = controller;
//                Invoke("doPlacement", 0.0f);
//                gameObject.SetActive(false);
//                break;
//            }
//        }
//    }

//    void doPlacement()
//    {
//        PlacementController.StartPlacement(confirmedController, charactersToPlace[confirmedController.placementSelectionIndex]);
//    }

//    public static void SetControllers(List<AvatarController> controllers)
//    {
//        instance.controllers = controllers;
//    }

//    public static void Show(List<Character> charactersToPlace)
//    {
//        instance.show(charactersToPlace);
//    }

//    private void show(List<Character> charactersToPlace)
//    {
  

//        this.charactersToPlace = charactersToPlace;
//        this.length = charactersToPlace.Count;

//        foreach (AvatarController controller in controllers)
//        {
//            controller.placementSelectionIndex = NeverdawnUtility.RepeatIndex(controller.placementSelectionIndex, length);
//        }

//        buttons.Clear();

//        foreach (Transform child in buttonParent)
//        {
//            Destroy(child.gameObject);
//        }

//        int k = 0;

//        foreach (Character character in charactersToPlace)
//        {
//            UIPlacementButton button = Instantiate(buttonPrefab);
//            button.character = character;
//            button.controllers = controllers;
//            button.index = k++;
//            button.icon = character.identity.icon;
//            button.transform.SetParent(buttonParent, false);

//            buttons.Add(button);
//        }

//        gameObject.SetActive(true);
//    }
//}

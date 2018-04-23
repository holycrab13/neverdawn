//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UINeverdawnParty : MonoBehaviour {

//    public GameObject characterMenuPrefab;

//    private static UINeverdawnParty instance;

//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//    }
	
//    public static void UpdateView()
//    {
//        if (instance != null)
//        {
//            Character[] characters = PlayerParty.GetAllCharacters();

//            foreach (Transform child in instance.transform)
//            {
//                Destroy(child.gameObject);
//            }

//            foreach (Character character in characters)
//            {
//                UIFighterIcon icon = Instantiate(instance.characterMenuPrefab).GetComponent<UIFighterIcon>();
//                icon.SetCharacter(character);

//                AvatarController controller = GameController.activeControllers.Find(c => c.character == character);

//                if(controller != null)
//                {
//                    icon.SetColor(controller.color);
//                }

//                icon.transform.SetParent(instance.transform, false);

//            }
//        }
//    }

//}

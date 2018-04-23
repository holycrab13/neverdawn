using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UICharacterMenus : MonoBehaviour {

    [SerializeField]
    private UICharacterMenu menuPrefab;

    private Dictionary<Character, UICharacterMenu> menus;

    private static UICharacterMenus instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            menus = new Dictionary<Character, UICharacterMenu>();
        }
    }

    public static UICharacterMenu GetMenu(Character character)
    {
        return instance.menus[character];
    }

    public static void UpdateView()
    {
        foreach (Character character in GameController.instance.party)
        {
            if (instance.menus.ContainsKey(character))
            {
                continue;
            }
            else
            {
                UICharacterMenu menu = Instantiate(instance.menuPrefab);
                menu.transform.SetParent(instance.transform, false);
                menu.character = character;
                
                instance.menus.Add(character, menu);
            }
        }

        List<UICharacterMenu> menusToRemove = new List<UICharacterMenu>();

        foreach (UICharacterMenu menu in instance.menus.Values)
        {
            if (!GameController.instance.party.Contains(menu.character))
            {
                menusToRemove.Add(menu);
            }
        }

        foreach (UICharacterMenu menu in menusToRemove)
        {
            instance.menus.Remove(menu.character);
            Destroy(menu);
        }

        RectTransform rectTransform = instance.GetComponent<RectTransform>();

        //int k = 0;
        //float size = rectTransform.sizeDelta.x / instance.menus.Count;

        //foreach (UICharacterMenu menu in instance.menus.Values)
        //{
        //    RectTransform menuRectTransform = menu.GetComponent<RectTransform>();

        //    menuRectTransform.sizeDelta = new Vector2(size, 0.0f);
        //    menuRectTransform.position = new Vector3(k * size, 0.0f);

        //    k++;
        //}
    }
}

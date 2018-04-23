using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINeverdawnEnemies : MonoBehaviour {

    public UISequenceIcon enemyMenuPrefab;

    private static UINeverdawnEnemies instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void SetCombatGroup(List<Character> group)
    {
        Clear();

        foreach(Character character in group)
        {
            UISequenceIcon icon = Instantiate(instance.enemyMenuPrefab);
            icon.character = character;
            icon.transform.SetParent(instance.transform);
        }

    }

    internal static void Clear()
    {
        foreach(Transform transform in instance.transform)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }
}

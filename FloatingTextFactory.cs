using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextFactory : MonoBehaviour {

    public TMP_FontAsset font;

    public int size = 32;

    private static FloatingTextFactory instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void Create(string text, Vector3 position, Color color, int size = 32, float duration = 2.0f, float scrollSpeed = 50.0f)
    {
        Create(text, position, color, size, duration, scrollSpeed, duration / 2.0f, duration / 2.0f);
    }

    public static void Create(string text, Vector3 position, Color color, int size, float duration, float scrollSpeed, float fadeDuration, float fadeOffset)
    {
        GameObject go = new GameObject("FloatingText_" + text);
       
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.font = instance.font;
        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = true;
        tmp.color = color;

        FloatingText floatingText = go.AddComponent<FloatingText>();
        floatingText.worldPosition = position;
        floatingText.scrollSpeed = scrollSpeed;
        floatingText.fadeDuration = fadeDuration;
        floatingText.fadeOffset = fadeOffset;

        go.transform.SetParent(instance.transform);

        Destroy(go, duration);
    }

	
}

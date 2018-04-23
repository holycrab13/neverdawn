using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour {

    public Vector3 worldPosition;
    public float scrollSpeed;
    public float fadeDuration;
    public float fadeOffset;

    private float x;
    private TextMeshProUGUI tmp;
    private Color color;
    private float initialAlpha;
	
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        color = tmp.color;
        initialAlpha = color.a;
    }
	// Update is called once per frame
	void Update () {
        x += Time.deltaTime;

        transform.position = Camera.main.WorldToScreenPoint(worldPosition) + x * scrollSpeed * Vector3.up;
        color.a = Mathf.Min(initialAlpha, -(1.0f / fadeDuration) * (x - fadeOffset) + initialAlpha);

        tmp.color = color;
	}
}

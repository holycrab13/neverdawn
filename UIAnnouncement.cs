using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnnouncement : MonoBehaviour {

    [SerializeField]
    private Text label;

    [SerializeField]
    private CanvasGroup group;

    [SerializeField]
    private float fadeDuration;

    private static UIAnnouncement instance;

    private float time;


	// Use this for initialization
	void Start () {

        if (instance == null)
        {
            instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () {

        time -= Time.deltaTime;

        group.alpha = Mathf.Clamp01(time / fadeDuration);
     
	}

    public static void Announce(string text, float seconds)
    {
        instance.label.text = text;
        instance.time = seconds + instance.fadeDuration;
    }
}

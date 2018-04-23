using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITime : MonoBehaviour {

    private TMP_Text text;

	// Use this for initialization
	void Awake () {

        text = GetComponent<TMP_Text>();
	}
	
	// Update is called once per frame
	void Update () {

        text.text = PlayerTime.GetTimeString();
	}
}

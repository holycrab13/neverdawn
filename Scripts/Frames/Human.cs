using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : FrameComponent {

    [SerializeField]
    private int _face;

    [SerializeField]
    private int _hair;

    [Range(1.0f, 2.5f)]
    [SerializeField]
    private float _height;

    [SerializeField]
    private Color _skinColor;

    [SerializeField]
    private bool _isChinese;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporary : MonoBehaviour {

    [SerializeField]
    private float lifetime;

	// Use this for initialization
	void Start () {

        Destroy(gameObject, lifetime);
	}
	
}

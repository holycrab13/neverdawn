using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour {

    Transform transformComponent;

    [SerializeField]
    private float rotationSpeed;

	// Use this for initialization
	void Start () {

        transformComponent = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

        transformComponent.eulerAngles = new Vector3(
            transformComponent.eulerAngles.x,
            transformComponent.eulerAngles.y + Time.deltaTime * rotationSpeed, 
            transformComponent.eulerAngles.z
            );

	}
}

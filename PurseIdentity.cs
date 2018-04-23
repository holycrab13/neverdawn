using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Identity))]
[RequireComponent(typeof(Purse))]
public class PurseIdentity : MonoBehaviour {

    private Identity identity;
    private Purse purse;

    [SerializeField]
    private string labelSuffix = "gold coins";

	// Use this for initialization
	void Start () {

        identity = GetComponent<Identity>();
        purse = GetComponent<Purse>();

        identity.label = purse.gold + " " + labelSuffix; 
	}
	
}

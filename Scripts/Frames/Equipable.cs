using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Castable))]
public class Equipable : FrameComponent {

    private bool m_isEquipped;

    public bool isEquipped
    {
        get { return m_isEquipped; }
        set { m_isEquipped = value; }
    }

    private Castable _castable;

    public AbilityBase[] abilities
    {
        get
        {
            if (_castable == null)
            {
                _castable = GetComponent<Castable>();
            }

            return _castable.abilities;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

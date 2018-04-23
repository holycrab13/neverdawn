using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static float gravity { get; private set; }

    public static float armorMitigationScale { get; private set; }

    public static Color neutralColor { get; private set; }

    public float m_gravity;

    public float _armorMitigationScale;

    public Color _neutralColor;
	// Use this for initialization
	void Awake () {

        gravity = m_gravity;
        armorMitigationScale = _armorMitigationScale;
        neutralColor = _neutralColor;
	}
	
}

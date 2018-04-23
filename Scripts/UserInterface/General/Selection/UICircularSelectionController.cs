using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICircularSelectionController : MonoBehaviour {

    public UISelectable center;

    public UISelectable[] peripherals;

    private Vector2 previousInput;

    public InputModule inputModule;

    public UISelectable selected { get; private set; }
    
    private float angle;

    void Start()
    {
        if(center != null)
        {
            center.Select();
        }

        if(peripherals != null && peripherals.Length > 0)
        {
            angle = 360.0f / peripherals.Length;
        }
    }

	// Update is called once per frame
	void Update () {

        if (inputModule != null)
        {
            float horizontal = inputModule.GetAxis(NeverdawnInputAxis.HorizontalLeft);
            float vertical = inputModule.GetAxis(NeverdawnInputAxis.VerticalLeft);

            Vector3 input = inputModule.normalizedDirection;

            if (input.sqrMagnitude < 0.1f)
            {
                if (center != null)
                {
                    center.Select();
                }
                else if(selected != null)
                {
                    selected.Deselect();
                }

                selected = null;
            }

            if(input.sqrMagnitude > 0.1f)
            {
                float inputAngle = Vector3.SignedAngle(Vector3.forward, input, Vector3.up);
                
                if(inputAngle < 0.0f)
                {
                    inputAngle = 360.0f + inputAngle;
                }

                inputAngle += angle / 2.0f;

                int index = NeverdawnUtility.RepeatIndex((int)(inputAngle / angle), peripherals.Length);

                selected = peripherals[index];
                peripherals[index].Select();
            }

            previousInput = input;
        }
    }
}

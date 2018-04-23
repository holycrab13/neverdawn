using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISemiCircularSelectionController : SelectionControllerBase
{
    public UISelectable center;

    public UISelectable[] peripherals;

    private Vector2 previousInput;

    public UISelectable selected { get; private set; }

    [SerializeField]
    private float arc;

    private float angle;

    void Start()
    {
        if(center != null)
        {
            center.Select();
        }

        if(peripherals != null && peripherals.Length > 0)
        {
            angle = arc / (peripherals.Length + 1);
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
                float inputAngle = Mathf.Clamp(-Vector3.SignedAngle(Vector3.forward, input, Vector3.up), -90.0f, 90.0f);
                
                inputAngle += angle / 2.0f;

                int index = Mathf.Clamp((int)(inputAngle / angle) + (peripherals.Length / 2), 0, peripherals.Length - 1);

                selected = peripherals[index];
                peripherals[index].Select();
            }

            previousInput = input;
        }
    }
}

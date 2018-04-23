using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Supporter : FrameComponent {

    [SerializeField]
    private float distance;

    [SerializeField]
    private string faction;
	
    public bool Call(Supporter supporter)
    {
        return supporter.faction == faction && Vector3.Distance(supporter.transform.position, transform.position) < distance;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}

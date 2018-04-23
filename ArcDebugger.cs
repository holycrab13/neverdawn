    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArcDebugger : MonoBehaviour {

    private Vector3 hitPosition;

    [SerializeField]
    private float precision;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 force = transform.forward * transform.localScale.z;

        RaycastHit hit;

        if (ProjectileUtils.BallisticCast(transform.position, force, out hit, precision))
        {
            hitPosition = hit.point;
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition, 0.1f);
    }
}

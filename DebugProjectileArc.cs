using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugProjectileArc : MonoBehaviour {

    public float angle;

    public Transform target;
    private float speed;
    private float distance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
      


   

	}

    void OnDrawGizmos()
    {     // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(target.position.x, 0, target.position.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - target.position.y;
      
        speed = ProjectileUtils.LaunchSpeed(distance, yOffset, 9.8f, angle * Mathf.Deg2Rad);
        Vector3 velocity = new Vector3(0, speed * Mathf.Sin(angle * Mathf.Deg2Rad), speed * Mathf.Cos(angle * Mathf.Deg2Rad));
        float angleBetweenObjects = Vector3.SignedAngle(Vector3.forward, planarTarget - planarPostion, Vector3.up);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        Gizmos.DrawLine(transform.position, transform.position + finalVelocity);

        Vector3 lastPosition = transform.position;

        for (float t = 0.0f; t < 10.0f; t += 0.05f)
        {
            Vector3 nextPosition = transform.position + finalVelocity * t + Vector3.down * (9.8f / 2.0f) * t * t;
            Gizmos.DrawLine(lastPosition, nextPosition);

            lastPosition = nextPosition;
        }
    }
}

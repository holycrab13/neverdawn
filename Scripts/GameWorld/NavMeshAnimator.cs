using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAnimator : MonoBehaviour {

    private Animator anim;

    public float MoveSmoothing = 0.2f;
    public float TurnSmoothing = 0.2f;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    private Transform m_transform;

    private Vector3 lastPos;
    private float smoothForward;

    public float moveSpeed = 5.0f;
    private float smoothTurn;
    public float turnSpeed = 540.0f;
    private Vector3 lastForward;
    private float turnBoost = 2.0f;


	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        m_transform = transform;

	}

    public void Reset(NavMeshAnimator previous = null)
    {
        if(m_transform == null)
        {
            m_transform = transform;
        }

        smoothForward = 0.0f;
        smoothTurn = 0.0f;
        lastPos = m_transform.position;
        lastForward = m_transform.forward;
    }
	
	// Update is called once per frame
    void Update()
    {

        Vector3 worldDeltaPosition = m_transform.position - lastPos;
        Vector3 worldForward = m_transform.forward;

        float worldDeltaRotationY = Vector3.SignedAngle(lastForward, worldForward, Vector3.up);

        // Map 'worldDeltaPosition' to local space
        float forward = (Vector3.Dot(m_transform.forward, worldDeltaPosition) / Time.deltaTime) / moveSpeed;
        float turn = ((worldDeltaRotationY / Time.deltaTime) / turnSpeed) * turnBoost * (2.0f - forward);

        smoothForward = Mathf.Lerp(smoothForward, forward, Time.deltaTime * (1.0f / MoveSmoothing));
        smoothTurn = Mathf.Lerp(smoothTurn, turn, Time.deltaTime * (1.0f / TurnSmoothing));
       
        velocity = smoothDeltaPosition;

        // Update animation parameters
        anim.SetFloat("Forward", smoothForward);
        anim.SetFloat("Turn", smoothTurn);

        lastPos = m_transform.position;
        lastForward = m_transform.forward;
    }


	
}

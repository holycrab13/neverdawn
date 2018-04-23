using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class CharacterMoveAlongPathAction : CharacterActionBase
{
    private Vector3 target;
    private bool run;
    private Transform transform;

    private Vector3[] corners;
    private float[] distances;
    private float distance;
    private float maxDistance;
    private float moveSpeed;
    private float turnSpeed;

    public CharacterMoveAlongPathAction(Vector3[] corners, float maxDistance, float smoothness = 1.0f, bool run = false)
    {
        this.corners = corners;
        this.distances = NeverdawnUtility.CreateDistanceArray(this.corners);
        this.maxDistance = maxDistance;
        this.run = run;
    }

    public CharacterMoveAlongPathAction(NavMeshPath path, float maxDistance, float smoothness = 1.0f, bool run = false)
        : this(path.corners, maxDistance, smoothness, run)
    {
       
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        transform = actor.transform;

        NavMeshAnimator walker = actor.GetComponentInChildren<NavMeshAnimator>();

        if(walker != null)
        {
            moveSpeed = walker.moveSpeed;
            turnSpeed = walker.turnSpeed;
        }
        else
        {
            moveSpeed = 5.0f;
            turnSpeed = 540.0f;
        }

        if(!run)
        {
            moveSpeed /= 2.0f;
        }

        float distance = Mathf.Min(NeverdawnUtility.GetPathLength(corners), maxDistance);

        distance = 0.0f;
    }

    public override void ActionUpdate(float timekey)
    {
        Vector3 position;

        float preferredDistance = distance + timekey * moveSpeed;

        if (!NeverdawnUtility.SamplePath(corners, distances, preferredDistance, out position) || preferredDistance > maxDistance)
        {
            Done();
            return;
        }

        Vector3 dir = (position - transform.position);
        dir.y = 0.0f;
        dir.Normalize();

        Vector2 fwdTurn = NeverdawnUtility.InputToForwardTurn(character.transform, dir, 1.0f);

        Vector3 forward = character.transform.forward;
        character.transform.forward = Vector3.RotateTowards(forward, dir, timekey * Mathf.PI * 2.0f * (turnSpeed / 360.0f), 0.0f);

        distance = distance + fwdTurn.x * timekey * moveSpeed;

        NeverdawnUtility.SamplePath(corners, distances, distance, out position);

        character.transform.position = position;

        RaycastHit rayHit;

        // experimental: stick to ground
        if (Physics.Raycast(character.transform.position + Vector3.up,Vector3.down, out rayHit, 3f, 1 << 9))
        {
            character.transform.position = rayHit.point;
        }
            
       
    }

}



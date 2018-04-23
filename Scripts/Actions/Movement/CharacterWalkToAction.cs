using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterWalkToAction : CharacterActionBase
{
    public Vector3 target;
    public Vector3 direction;
    private float turn;
    private float forward;
    private CharacterTurnAction turnTo;
    private bool turnToDone;
    private float turnSpeed;
    private float distance;
    private float moveSpeed;
    private float maxDistance;


    public CharacterWalkToAction(Vector3 target)
    {
        this.target = target;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        NavMeshAnimator walker = actor.GetComponentInChildren<NavMeshAnimator>();

        if (walker != null)
        {
            moveSpeed = walker.moveSpeed;
            turnSpeed = walker.turnSpeed;
        }
        else
        {
            moveSpeed = 5.0f;
            turnSpeed = 540.0f;
        }

        direction = (target - actor.position);
        direction.Normalize();


        maxDistance = (target - actor.position).magnitude;
        distance = 0.0f;
    }

    public override void ActionUpdate(float timekey)
    {

        Vector2 fwdTurn = NeverdawnUtility.InputToForwardTurn(character.transform, direction, 1.0f);

        Vector3 forward = character.transform.forward;
        character.transform.forward = Vector3.RotateTowards(forward, direction, timekey * Mathf.PI * 2.0f * (turnSpeed / 360.0f), 0.0f);

        distance += fwdTurn.x * timekey * moveSpeed;
        character.transform.position += direction * fwdTurn.x * timekey * moveSpeed;

        if (distance >= maxDistance)
        {
            Done();
        }
    }

}


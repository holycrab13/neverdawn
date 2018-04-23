using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterTurnTowardsAction : CharacterActionBase
{
    /// <summary>
    /// The normalized direction to the target
    /// </summary>
    private Vector3 targetDirection;
    
    /// <summary>
    /// The turn speed in radians per second
    /// </summary>
    private float angularSpeed;

    /// <summary>
    /// The angle threshold
    /// </summary>
    private float angleThreshold;

    /// <summary>
    /// The position to turn to
    /// </summary>
    private Vector3 target;

    /// <summary>
    /// Angluar speed in degree per second
    /// </summary>
    /// <param name="target"></param>
    /// <param name="angularSpeed"></param>
    /// <param name="angleThreshold"></param>
    public CharacterTurnTowardsAction(Vector3 target, float angularSpeed = 360.0f, float angleThreshold = 2.0f)
    {
        this.angularSpeed = Mathf.PI * (angularSpeed / 180.0f);
        this.angleThreshold = angleThreshold;
        this.target = target;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        if(Vector3.Distance(target, actor.position) < 0.1f)
        {
            Done();
            return;
        }

        targetDirection = target - actor.position;
        targetDirection.y = 0.0f;
        targetDirection.Normalize();

        float angle = Vector3.Angle(actor.forward, targetDirection);

        if (angle < angleThreshold)
        {
            Done();
        }
    }

    public override void ActionUpdate(float timekey)
    {
        character.forward = Vector3.RotateTowards(character.forward, targetDirection, angularSpeed * timekey, 0.0f);

        float angle = Vector3.Angle(character.forward, targetDirection);

        if (angle < angleThreshold)
        {
            Done();
        }
    }
}


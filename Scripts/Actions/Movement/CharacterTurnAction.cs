using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterTurnAction : CharacterActionBase
{
    private Vector3 targetDirection;
    private float angle;
    private float turnStep;
    private float targetAngle;
    private float startAngle;
    private float time;

    private float angularSpeed;
    private float angleThreshold;

    public CharacterTurnAction(Vector3 direction, float angularSpeed = 360.0f, float angleThreshold = 2.0f)
    {
        this.angularSpeed = Mathf.PI * (angularSpeed / 180.0f);
        this.targetDirection = direction;
        this.angleThreshold = angleThreshold;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        targetDirection.y = 0.0f;
        targetDirection.Normalize();

        if(targetDirection.magnitude == 0.0f)
        {
            Done();
            return;
        }

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


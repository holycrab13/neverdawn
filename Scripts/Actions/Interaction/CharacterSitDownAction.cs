using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterSitDownAction : CharacterActionBase
{
    private Transform target;

    private Animator animator;

    private Vector3 entryPoint;

    private Collider collider;

    private bool sittingDown;

    private float positionLerp;

    private CharacterNavigateToAction walkTo;

    private CharacterTurnAction turnTo;

    private CharacterActionOnce standUp;

    private CharacterActionOnce sitDown;

    public CharacterSitDownAction(Vector3 entryPoint, Transform target)
    {
        this.entryPoint = entryPoint;
        this.target = target;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        animator = actor.GetComponent<Animator>();
        collider = actor.GetComponent<Collider>();

        sitDown = new CharacterActionOnce();
        standUp = new CharacterActionOnce();

        walkTo = new CharacterNavigateToAction(this.entryPoint, float.MaxValue, false);
        turnTo = new CharacterTurnAction(target.forward, 180.0f);

        sittingDown = true;
    }

    public override void ActionUpdate(float timekey)
    {
        if (!walkTo.IsStarted)
        {
            walkTo.ActionStart(character);
            return;
        }

        if (!walkTo.IsDone)
        {
            walkTo.ActionUpdate(timekey);
            return;
        }

        if (!turnTo.IsStarted)
        {
            turnTo.ActionStart(character);
            return;
        }

        if (!turnTo.IsDone)
        {
            turnTo.ActionUpdate(timekey);
            return;
        }

        if (sittingDown)
        {
            if (sitDown.Once())
            {
                collider.isTrigger = true;
                entryPoint = character.transform.position;

                animator.SetFloat("Turn", 0.0f);
                animator.SetFloat("Forward", 0.0f);
                animator.SetTrigger("SitDown");
            }

            if (positionLerp < 1.0f)
            {
                positionLerp += timekey * .75f;
                character.transform.position = Vector3.Lerp(entryPoint, target.position, positionLerp);
                return;
            }

            sittingDown = false;
        }

        if (IsCancelled)
        {
            if (standUp.Once())
            {
                animator.SetTrigger("StandUp");
                positionLerp = 0.0f;
            }

            if (positionLerp < 1.0f)
            {
                positionLerp += timekey * .75f;
                character.transform.position = Vector3.Lerp(target.position, entryPoint, positionLerp);
                return;
            }
            else
            {
                collider.isTrigger = false;
                Done();
            }
        }
    }
}


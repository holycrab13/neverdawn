using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class CharacterNavigateToAction : CharacterActionBase
{

    private Vector3 target;
    private bool run;
    private Transform transform;
    private CharacterMoveAlongPathAction action;
    private float walkDistance;
    private float stoppingDistance;


    public CharacterNavigateToAction(Vector3 position, float distance = float.MaxValue, bool run = false, float stoppingDistance = 0.01f)
    {
        this.target = position;
        this.run = run;
        this.walkDistance = distance;
        this.stoppingDistance = stoppingDistance;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        transform = actor.transform;

        NavMeshPath path = new NavMeshPath();

        if (NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path))
        {
            Vector3[] line = NeverdawnUtility.RelaxPath(NeverdawnUtility.RefinePath(path.corners, 0.1f), 10);
            float pathLength = NeverdawnUtility.GetPathLength(line);

            if (pathLength - stoppingDistance < walkDistance)
                walkDistance = pathLength - stoppingDistance;
           
            action = new CharacterMoveAlongPathAction(line, walkDistance, 1.0f, run);
            action.ActionStart(actor);
        }
        else
        {
            Done();
        }
    }

    public override void ActionUpdate(float timekey)
    {
        if (action.IsDone)
        {
            Done();
            return;
        }

        action.ActionUpdate(timekey);

    }
}



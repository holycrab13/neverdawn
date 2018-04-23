using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class CharacterNavigateToTileAction : CharacterActionBase
{

    private Vector3 target;
    private bool run;
    private Transform transform;
    private CharacterMoveAlongPathAction action;

    private  HexTile targetTile;
    private bool includeLast;
    private bool ignoreTarget;


    public CharacterNavigateToTileAction(HexTile targetTile, bool ignoreTarget = false, bool includeLast = false, bool run = false)
    {
        this.targetTile = targetTile;
        this.run = run;
        this.includeLast = includeLast;
        this.ignoreTarget = ignoreTarget;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        transform = actor.transform;

        HexTile startTile = HexTerrain.GetClosestTile(character.position);

        if(startTile == null) 
        {
            Done();
            return;
        }

        List<HexTile> path = Pathfinder.FindPath(startTile, targetTile, ignoreTarget ? PathFindingMode.ExcludeTarget : PathFindingMode.None);

        if(path == null)
        {
            Done();
            return;
        }

        if (actor.remainingSteps == 0)
        {
            Done();
            return;
        }

        if (!includeLast)
        {
            path.Remove(targetTile);
        }


        while (path.Count > actor.remainingSteps + 1)
        {
            path.RemoveAt(path.Count - 1);
        }

        targetTile = path[path.Count - 1];
        actor.remainingSteps -= path.Count - 1;

        Vector3[] line = NeverdawnUtility.PathAlongTiles(path, 0.1f, 10);
        float pathLength = NeverdawnUtility.GetPathLength(line);

        action = new CharacterMoveAlongPathAction(line, 1000.0f, 1.0f, run);
        action.ActionStart(actor);
    }
        
    public override void ActionUpdate(float timekey)
    {
        if (action.IsDone)
        {
            character.UpdateTile();
            Done();
            return;
        }

        action.ActionUpdate(timekey);

    }
}



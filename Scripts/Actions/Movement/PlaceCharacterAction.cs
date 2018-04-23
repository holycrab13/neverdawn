using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class PlaceCharacterAction : CharacterActionBase
{

    private Vector3 target;
    private Transform transform;
    private CharacterMoveAlongPathAction action;

    private  HexTile targetTile;
    private Character characterToPlace;


    public PlaceCharacterAction(HexTile targetTile, Character characterToPlace)
    {
        this.targetTile = targetTile;
        this.characterToPlace = characterToPlace;
    }

    protected override void OnCharacterActionStart(Character actor)
    {
        transform = actor.transform;

        HexTile startTile = actor.currentTile;

        if(startTile == null) 
        {
            Done();
            return;
        }

        List<HexTile> path = Pathfinder.FindPath(startTile, targetTile, PathFindingMode.None);

        if(path == null)
        {
            Done();
            return;
        }

        Vector3[] line = NeverdawnUtility.PathAlongTiles(path, 0.1f, 10);
        float pathLength = NeverdawnUtility.GetPathLength(line);


        GameController.instance.party.PullCharacter(characterToPlace, targetTile.position);

        action = new CharacterMoveAlongPathAction(line, 1000.0f, 1.0f, false);
        action.ActionStart(characterToPlace);
    }
        
    public override void ActionUpdate(float timekey)
    {
        if (action.IsDone)
        {
            characterToPlace.EnterCombatStance();
            Done();
            return;
        }

        action.ActionUpdate(timekey);

    }
}



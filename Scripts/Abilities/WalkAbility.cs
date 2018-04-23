using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Neverdawn/Abilities/Walk", order = 1)]
public class WalkAbility : AbilityBase
{
    [SerializeField]
    protected TileWalkCursor walkCursor;

    public float distanceModifier;

    public float maxDistance;

    private Character walker;

    protected TileWalkCursor cursor;

    private HexTile targetTile { get; set; }

    public override void ApplyCursor()
    {
        targetTile = cursor.targetTile;
    }

    protected override bool isCastable(Character character)
    {
        return walker.remainingSteps > 0;
    }

    public override CharacterActionBase Cast()
    {
        return new DoNothing();
    }

    public override bool Initialize(GameObject gameObject)
    {
        walker = gameObject.GetComponent<Character>();

        if (walker != null)
        {
            return true;
        }

        return false;
    }

    protected override AbilityCursorBase createCursor()
    {
        maxDistance = walker.remainingSteps;

        cursor = Instantiate(walkCursor);

        return cursor; 
    }

    public List<Character> charactersToPlace { get; set; }
}

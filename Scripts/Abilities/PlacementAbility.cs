using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Neverdawn/Abilities/Placement", order = 1)]
public class PlacementAbility : AbilityBase
{
    [SerializeField]
    protected PlacementCursor placementCursor;

    public float distanceModifier;

    private Character walker;

    protected PlacementCursor cursor;

    private HexTile targetTile { get; set; }

    private Character characterToPlace { get; set; }

    public override void ApplyCursor()
    {
        targetTile = cursor.targetTile;
        characterToPlace = cursor.characterToPlace;
    }

    protected override bool isCastable(Character character)
    {
        return true;
    }

    public override CharacterActionBase Cast()
    {
        return new PlaceCharacterAction(targetTile, characterToPlace);
    }

    public override bool Initialize(GameObject gameObject)
    {
        characterToPlace = gameObject.GetComponent<Character>();
        return true;
    }

    protected override AbilityCursorBase createCursor()
    {
        cursor = Instantiate(placementCursor);
        cursor.characterToPlace = characterToPlace;
        return cursor;
    }

}

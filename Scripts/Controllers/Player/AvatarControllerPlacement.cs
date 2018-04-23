using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class AvatarController : NeverdawnCharacterController
{
    public int placementSelectionIndex { get; set; }

    /// <summary>
    /// Cursor for combat movement
    /// </summary>
    [SerializeField]
    private PlacementAbility placementAbility;

    internal void PlaceCharacter(Character character)
    {
        placementAbility.Initialize(character.gameObject);
        CastAbility(placementAbility);

        DestroyMarker();
    }

    public void UpdatePlacementControls()
    {
        if (!positionMarker)
        {
            positionMarker = HexTerrain.CreateTileSelector(color);
            positionMarker.transform.position = character.currentTile.position;
        }
    
        if (currentAbility == null)
        {
            if (!characterMenu.isOpen)
            {
                UIQuickMenuPlacement placementPage = Instantiate(UIFactory.uiQuickMenuPlacementPrefab);
                characterMenu.Open(placementPage);
            }   
        }
        else
        {
            updateAbility();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NeverdawnCharacterController : MonoBehaviour {

    protected GameObject positionMarker;

    public Color color { get; set; }

    private Character _character;

    public virtual Character character
    {
        get 
        {
            return _character; 
        }
        set
        {
            if (_character != null)
            {
                _character.controller = null;
            }

            _character = value;

            if (value != null)
            {
                _character.controller = this;
                characterTransform = value.GetComponent<Transform>();
                characterAudioSource = value.GetComponentInChildren<AudioSource>();
                characterNavMeshAnimator = value.GetComponentInChildren<NavMeshAnimator>();
            }
        }
    }

    protected Transform characterTransform { get; private set; }

    protected AudioSource characterAudioSource { get; private set; }

    protected NavMeshAnimator characterNavMeshAnimator { get; private set; }

    public virtual void Loot(Container container)
    {
        // container.TakeAllItems(this);
    }

    public virtual void StartCombatTurn()
    {
        character.StartTurn();
    }

    public virtual void UpdateCombatControls()
    {
        if (!positionMarker)
        {
            positionMarker = HexTerrain.CreateTileSelector(color);
        }

        positionMarker.transform.position = character.currentTile.position;
    }


    public virtual void ChoseInteraction(Interactable frame)
    {
        Interactable interactable = frame.GetComponent<Interactable>();

        if (interactable != null && interactable.interactions.Length > 0)
        {
            interactable.interactions[0].Interact(this);
        }
    }

    protected void EndCombatTurn()
    {
        DestroyMarker();
        GameController.instance.combatController.EndTurn(this);
    }

    internal void DestroyMarker()
    {
        if (positionMarker)
        {
            Destroy(positionMarker);
        }
    }
}

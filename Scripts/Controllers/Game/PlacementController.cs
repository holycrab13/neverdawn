using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.AI;

public class PlacementController : MonoBehaviour {

    [SerializeField]
    private WalkAbility placementAbility;

    public UnityEvent onPlacementComplete;

    private static List<Character> _charactersToPlace;

    public static List<Character> charactersToPlace
    {
        get { return _charactersToPlace; }
    }

    private List<AvatarController> activeControllers;

    private AvatarController activeController;
    
    private static PlacementController instance;

    private Character activeCharacter;

    private AbilityCursorBase placementCursor;

    void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Initialize the placement ui and fields
    /// </summary>
    /// <param name="controllers"></param>
    public void InitializePlacement(List<AvatarController> controllers)
    {
        activeControllers = new List<AvatarController>();
        activeControllers.AddRange(controllers.Where(c => c.character != null));

        _charactersToPlace = new List<Character>();
        _charactersToPlace.AddRange(GameController.instance.party.hiddenCharacters);

        if (_charactersToPlace.Count == 0)
        {
            onPlacementComplete.Invoke();
            return;
        }

        //UIPlacementMenu.SetControllers(activeControllers);
        //UIPlacementMenu.Show(_charactersToPlace);
    }

    public void UpdatePlacement()
    {
        if (_charactersToPlace != null)
        {

            if (_charactersToPlace.Count == 0)
            {
                onPlacementComplete.Invoke();
            }

            foreach (AvatarController activeController in activeControllers)
            {
                activeController.UpdatePlacementControls();
            }

            _charactersToPlace.RemoveAll(c => !GameController.instance.party.IsHidden(c));
        }
    }

    /// <summary>
    /// Place a character at the end of the given path, or at max distance along the given path
    /// </summary>
    /// <param name="characterToPlace"></param>
    /// <param name="path"></param>
    /// <param name="maxDistance"></param>
    private void placeCharacter(Character characterToPlace)
    {
        Destroy(placementCursor.gameObject);

        if (characterToPlace != null)
        {
            _charactersToPlace.Remove(characterToPlace);
        }

        if (_charactersToPlace.Count == 0)
        {
            onPlacementComplete.Invoke();
        }
    }

    private void startPlacement(AvatarController controller, Character character)
    {
        activeCharacter = character;
        activeController = controller;

        controller.character.solid.Hide();

        characterToPlace.position = controller.character.position;
        characterToPlace.eulerAngles = controller.character.eulerAngles;
        characterToPlace.GetComponentInChildren<NavMeshAnimator>().Reset();
        characterToPlace.solid.Show();

        //placementCursor = placementAbility.createCursor();
        //placementCursor.Initialize(controller.color, character);
    }

    public static Character characterToPlace
    {
        get { return instance.activeCharacter; }
    }
}

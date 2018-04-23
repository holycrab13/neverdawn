using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.AI;
using UnityEngine.Events;

public class CombatController : MonoBehaviour
{

    [SerializeField]
    private PlacementController placementController;

    [SerializeField]
    private AbilityBase _walkAbility;

    [SerializeField]
    private UnityEvent onVictory;

    [SerializeField]
    private UnityEvent onDefeat;

    private int turnIndex;

    public HashSet<HexTile> battlefield { get; private set; }

    public List<Character> enemies { get; private set; }

    public List<Character> allies { get; private set; }

    private List<Character> currentCombatGroup;

    private bool _combatStarted;

    private List<AvatarController> _controllers;

    private int auxiliaryControllerIndex;

    private Fighter[] supporter;

    private AvatarController activeController;

    private static CombatController instance;

    private List<NeverdawnCharacterController> currentTurnControllers;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        placementController.onPlacementComplete.AddListener(StartCombat);
    }

    public static AbilityBase walkAbility
    {
        get { return instance._walkAbility; }
    }


    public void AddBattlefieldArea(HexTile center, int range)
    {
        if (battlefield == null)
        {
            battlefield = new Battlefield();;
        }

      

        battl
    }

    private void updateCurrentControllers()
    {
        currentTurnControllers.Clear();

        Character character = currentCombatGroup[turnIndex];

        // find the next living character
        while (!character.isAlive)
        {
            turnIndex = NeverdawnUtility.RepeatIndex(turnIndex + 1, currentCombatGroup.Count);
            character = currentCombatGroup[turnIndex];
        }

        // fetch all characters in the party in the following sequence
        if (character.IsInPlayerParty)
        {
            while (character.IsInPlayerParty)
            {
                // find the current controller or an auxiliary controller
                NeverdawnCharacterController controller = character.controller;

                if (!controller)
                {
                    controller = findControllerForCharacter(character);
                }

                // collect the controllers and try to add more characters
                if (!currentTurnControllers.Contains(controller))
                {
                    controller.character = character;

                    currentTurnControllers.Add(controller); 

                    turnIndex = NeverdawnUtility.RepeatIndex(turnIndex + 1, currentCombatGroup.Count);
                    character = currentCombatGroup[turnIndex];
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            currentTurnControllers.Add(character.controller);
            turnIndex = NeverdawnUtility.RepeatIndex(turnIndex + 1, currentCombatGroup.Count);
        }

        NeverdawnCamera.Clear();
        currentTurnControllers.ForEach(c => NeverdawnCamera.AddTargetLerped(c.character.cachedTransform));
    }

    private NeverdawnCharacterController findControllerForCharacter(Character character)
    {
        AvatarController preferredController = _controllers.FirstOrDefault(c => c.preferredCharacterId == character.id);

        if (preferredController)
        {
            return preferredController;
        }
        else
        {
            NeverdawnCharacterController auxiliaryController = _controllers[auxiliaryControllerIndex];
            auxiliaryControllerIndex = NeverdawnUtility.RepeatIndex(auxiliaryControllerIndex + 1, _controllers.Count);
            return auxiliaryController;
        }
    }

    public void PrepareCombat(List<AvatarController> controllers, Character attacker)
    {
        _controllers = controllers;
        _combatStarted = false;

        enemies = assembleEnemyParty(attacker);
        enemies.ForEach(e => e.EnterCombatStance());
        GameController.instance.party.activeCharacters.ForEach(c => c.EnterCombatStance());

        StartCoroutine(startPlacement(2.0f));
    }

    /// <summary>
    /// Start the actual turn based fighting
    /// </summary>
    public void StartCombat()
    {
        currentTurnControllers = new List<NeverdawnCharacterController>();

        allies = GameController.instance.party.activeCharacters;

        currentCombatGroup = new List<Character>();
        currentCombatGroup.AddRange(enemies);
        currentCombatGroup.AddRange(allies);
        currentCombatGroup = currentCombatGroup.OrderByDescending(c => c.initiative).ToList();

        UINeverdawnEnemies.SetCombatGroup(currentCombatGroup);

        _combatStarted = true;
    }

    private IEnumerator startPlacement(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        placementController.InitializePlacement(_controllers);
    }

    public float EvaluateSituation(IEnumerable<Character> party, IEnumerable<Character> enemies)
    {
        float partyScore = 0.0f;
        float enemyScore = 0.0f;
        float clusterScore = 0.0f;

        float partyWeight = 0.4f;
        float enemyWeight = 0.5f;
        float clusterWeight = 0.05f;

        foreach (Character character in party)
        {
            partyScore += character.GetCombatScore();
        }

        foreach (Character character in enemies)
        {
            enemyScore -= character.GetCombatScore();
        }

        foreach(Character c1 in party)
        {
            foreach(Character c2 in party)
            {
                clusterScore -= Vector3.Distance(c1.position, c2.position) / 100.0f;
            }
        }

        return partyScore * partyWeight + enemyScore * enemyWeight + clusterScore * clusterWeight;
    }
 
    public void UpdateCombat()
    {
        if (_combatStarted)
        {
            for (int i = currentTurnControllers.Count - 1; i >= 0; i--)
            {
                currentTurnControllers[i].UpdateCombatControls();
            }
               
            if(enemies.All(e => !e.isAlive))
            {
                foreach (NeverdawnCharacterController controller in currentTurnControllers)
                {
                    controller.DestroyMarker();
                }

                if (onVictory != null)
                {
                    onVictory.Invoke();
                }
                
                return;
            }

            if (allies.All(e => !e.isAlive))
            {
                if (onDefeat != null)
                {
                    onDefeat.Invoke();
                }

                return;
            }

            // all controllers finished turn, get new controllers!
            if (currentTurnControllers.Count == 0)
            {
                updateCurrentControllers();

                foreach (NeverdawnCharacterController controller in currentTurnControllers)
                {
                    controller.StartCombatTurn();
                }
            }
        }
        else
        {
            placementController.UpdatePlacement();
        }
    }

    private List<Character> assembleEnemyParty(Character fighter)
    {
        Fighter caller = fighter.GetComponent<Fighter>();
        List<Character> enemyParty = new List<Character>();
        enemyParty.Add(fighter);

        if (caller != null && !string.IsNullOrEmpty(caller.faction))
        {
            foreach (Fighter supp in FindObjectsOfType<Fighter>())
            {
                if (supp != caller && supp.Call(caller))
                {
                    enemyParty.Add(supp.GetComponent<Character>());
                }
            }
        }

        return enemyParty;
    }


    internal void EndTurn(NeverdawnCharacterController characterController)
    {
        currentTurnControllers.Remove(characterController);
    }

    internal void ExitCombat()
    {
        foreach (Character character in currentCombatGroup)
        {
            if (character.isAlive)
            {
                character.LeaveCombatStance();
            }
        }

        NeverdawnCamera.Clear();

        foreach (AvatarController controller in _controllers)
        {
            controller.character = Frame.FindComponentById<Character>(controller.preferredCharacterId);
            NeverdawnCamera.AddTargetLerped(controller.character.transform);
        }

        GameController.instance.party.StoreUnused();
    }

    internal bool IsCharacterTurn(Character character)
    {
        return currentTurnControllers != null && currentTurnControllers.Any(c => c.character == character);
    }

    internal void AddEnemy(Character neverdawnCharacter)
    {
        //enemies = enemies.Concat(assembleEnemyParty(neverdawnCharacter)).ToArray();
    }

    internal Character[] GetEnemiesInRange(Vector3 vector3, float p)
    {
        return enemies.Where(e => Vector3.Distance(e.position, vector3) <= p).ToArray();
    }

    internal Character[] GetEnemiesInRange(HexTile hexTile, int maxRange)
    {
        List<Character> enemiesInRange = new List<Character>();

        if (enemies != null)
        {
            foreach (Character enemy in enemies)
            {
                int distance = Pathfinder.Distance(hexTile, enemy.currentTile);

                if (distance >= 0 && distance <= maxRange)
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        return enemiesInRange.ToArray();
    }

}

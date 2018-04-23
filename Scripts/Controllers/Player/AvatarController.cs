using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class AvatarController : NeverdawnCharacterController
{
    /// <summary>
    /// The current interaction target
    /// </summary>
    private Interactable currentInteractable;


    /// <summary>
    /// The current default character
    /// </summary>
    public string preferredCharacterId { get; set; }

    /// <summary>
    /// When setting the character, also change access to the character menu
    /// </summary>
    public override Character character
    {
        get
        {
            return base.character;
        }
        set
        {
            base.character = value;

            if (value != null)
            {
                characterMenu = UICharacterMenus.GetMenu(character);
            }
        }
    }

    public InputModule inputModule { get; set; }

    /// <summary>
    /// The quick menu UI
    /// </summary>
    public UICharacterMenu characterMenu { get; set; }



    private UIQuickMenuQuickSlots quickSlotsPage;

    private Vector3 _lastInput;

    [SerializeField]
    private float _inputSmoothing = 2f * Mathf.PI;
    private Vector3 smoothedInput;

    /// <summary>
    /// Initialize!
    /// </summary>
    public void Initialize()
    {
        
      
    }
    
    private void updateInteraction(Interactable interactable)
    {
        if (currentInteractable != interactable)
        {
            if (currentInteractable != null)
            {
                currentInteractable.Deselect();
            }

            currentInteractable = interactable;

            if (currentInteractable != null)
            {
                currentInteractable.Select();
            }
        }
    }

    internal bool UpdateChatControls()
    {
        // Cancel pressed
        if (inputModule.GetButtonDown(NeverdawnInputButton.Cancel))
        {
            return false;
        }

        return true;
    }

    public void UpdateExplorationControls()
    {
        if (character != null)
        {
       

            if (currentAbility != null)
            {
                updateAbility();
                return;
            }

            if (inputModule.GetAxisUp(NeverdawnInputAxis.Trigger, NeverdawnInputAxisDirection.Positive))
            {
                if (quickSlotsPage != null)
                {
                    quickSlotsPage = null;
                    characterMenu.Close();
                }
            }

           

            if (inputModule.GetAxisDown(NeverdawnInputAxis.Trigger, NeverdawnInputAxisDirection.Positive))
            {
                if (character.isIdle)
                {
                    quickSlotsPage = Instantiate(UIFactory.uiQuickMenuQuickSlotsPrefab);
                    quickSlotsPage.abilities = character.quickCastAbilities;

                    characterMenu.Open(quickSlotsPage);
                }
            }

            if (characterMenu.isOpen)
            {
                if (inputModule.GetButtonDown(NeverdawnInputButton.Cancel))
                {
                    characterMenu.GoBack();
                }

                return;
            }

            // DEBUG LVL UP
            if (inputModule.GetButtonDown(NeverdawnInputButton.Info))
            {
                character.LevelUp();
            }

            // Start pressed
            if (inputModule.GetButtonDown(NeverdawnInputButton.Start))
            {
                if (character.isIdle)
                {
                    characterMenu.NavigateInto(Instantiate(UIFactory.uiQuickMenuStartPrefab));
                }
            }

            //Select pressed
            if (inputModule.GetButtonDown(NeverdawnInputButton.Settings))
            {
                if (character.isIdle)
                {
                    GameController.instance.PauseGame();
                }
            }

            // Confirm pressed
            if (inputModule.GetButtonDown(NeverdawnInputButton.Confirm))
            {
                if (character.isIdle)
                {
                    if (currentInteractable != null)
                    {
                        UIQuickMenuInteractable page = Instantiate(UIFactory.uiQuickMenuInteractionCollectionPrefab);
                        page.SetInteractionCollection(currentInteractable);

                        characterMenu.NavigateInto(page);
                    }
                }
                else
                {
                    character.currentAction.ActionConfirm();
                }
            }

            // Cancel pressed
            if (inputModule.GetButtonDown(NeverdawnInputButton.Cancel))
            {
                if (!character.isIdle)
                {
                    character.currentAction.ActionCancel();
                }
                else
                {
                    characterMenu.NavigateInto(Instantiate(UIFactory.uiQuickMenuStartPrefab));
                }
            }

            // Switch character pressed
            if (inputModule.GetButtonDown(NeverdawnInputButton.SwitchCharacter))
            {
                if (character.isIdle)
                {
                    NeverdawnCamera.RemoveTargetLerped(characterTransform);

                    Character nextCharacter = GameController.instance.party.GetNextCharacter(this);

                    if (nextCharacter != null)
                    {
                        character = nextCharacter;
                        preferredCharacterId = nextCharacter.id;
                    }


                    NeverdawnCamera.AddTargetLerped(characterTransform);

                    // UINeverdawnParty.UpdateView();

                    characterMenu = UICharacterMenus.GetMenu(character);
                }
            }

            // Movement
            if (character.isIdle)
            {

                Vector3 input = inputModule.normalizedDirection;
                smoothedInput = Vector3.RotateTowards(smoothedInput, input, Time.deltaTime * _inputSmoothing, 1.0f);
             
                // Get player input
                Vector2 fwdTurn = NeverdawnUtility.InputToForwardTurn(characterTransform, smoothedInput, 2.0f);

                // Rotate the player
                characterTransform.Rotate(Vector3.up, fwdTurn.y * Time.deltaTime * characterNavMeshAnimator.turnSpeed);

                // Find target position
                Vector3 targetPosition = characterTransform.position + characterNavMeshAnimator.moveSpeed * fwdTurn.x * characterTransform.forward * Time.deltaTime;

                NavMeshHit hit;

                // Check for valid position
                if (!NavMesh.Raycast(characterTransform.position, targetPosition, out hit, NavMesh.AllAreas))
                {
                    characterTransform.position = hit.position;
                }
                else
                {
                    if (NavMesh.SamplePosition(targetPosition, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        characterTransform.position = hit.position;
                    }
                }

                RaycastHit rayHit;

                // experimental: stick to ground
                if (Physics.Raycast(characterTransform.position, Vector3.down, out rayHit, 0.5f, 1 << 9))
                {
                    characterTransform.position = rayHit.point;
                }

            }

            // Find closest interaction
            Collider[] colliders = Physics.OverlapSphere(character.transform.position, 1.0f, 1 << 10);

            float minDistance = float.MaxValue;
            Interactable closestHandle = null;

            for (int i = 0; i < colliders.Length; i++)
            {
                float distance = Vector3.Distance(colliders[i].transform.position, character.transform.position);
                Interactable handle = colliders[i].GetComponentInParent<Interactable>();

                if (distance < minDistance && handle != null && handle.enabled)
                {
                    closestHandle = handle;
                    minDistance = distance;
                }
            }

            if (closestHandle != null)
            {
                updateInteraction(closestHandle);
            }
            else
            {
                updateInteraction(null);
            }
        }
    }
  
    public override void Loot(Container container)
    {
        UIQuickMenuLoot lootMenu = Instantiate(UIFactory.uiQuickMenuLootPrefab);

        lootMenu.container = container;
        characterMenu.NavigateInto(lootMenu);
    }

    public override void ChoseInteraction(Interactable frame)
    {
        UIQuickMenuInteractable page = Instantiate(UIFactory.uiQuickMenuInteractionCollectionPrefab);
        page.SetInteractionCollection(frame);

        characterMenu.NavigateInto(page);
    }

    /// <summary>
    /// Cast an ability (pick the target, if needed)
    /// </summary>
    /// <param name="abilityBase"></param>
    public void CastAbility(AbilityBase abilityBase)
    {
        if (character != null && abilityBase != null)
        {
            //if (GameController.state == GameState.Exploration)
            //{
            //    characterMenu.Close();
            //}

            character.UpdateTile();

            currentAbility = abilityBase;
            currentAbility.caster = character;
            currentAbility.CreateCursorForController(this);

            character.PushAction(currentAbility.Prepare());

            if (currentAbility.cursor == null)
            {
                character.PushAction(currentAbility.Cast());
                return;
            }
        }
    }


    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AISimpleCombatController : NeverdawnCharacterController {

    private float delay;

    private NavMeshObstacle obstacle;

    private Character target;


    void Start()
    {
        character = GetComponent<Character>();
        obstacle = GetComponentInChildren<NavMeshObstacle>(); 
        color = GameSettings.neutralColor;
    }

	// Update is called once per frame
	public override void UpdateCombatControls () {

        base.UpdateCombatControls();

        if (character.isIdle)
        {
            if (obstacle.enabled)
            {
                obstacle.enabled = false;
                return;
            }

            if (character.remainingActions > 0)
            {
                MeleeAttackAbility meleeAbility = character.GetCastableAbilities<MeleeAttackAbility>()
                    .Where(a => a.CharacterHasEnoughMana(character))
                    .OrderByDescending(m => m.threatRating).FirstOrDefault();

                if (meleeAbility != null)
                {
                    MeleeAttackAbility currentAbility = meleeAbility;

                    target = CharacterUtils.GetClosestCharacter(GameController.instance.party, character.position);

                    if (target != null)
                    {
                        if (character.currentTile.IsAdjacent(target.currentTile))
                        {
                            if (currentAbility.IsCastable(character))
                            {
                                currentAbility.caster = character;
                                currentAbility.target = target.frame;

                                character.PushAction(currentAbility.Prepare());
                                character.PushAction(currentAbility.Cast());
                            }
                        }
                        else
                        {
                            if (character.remainingSteps > 0)
                            {
                                character.PushAction(new CharacterNavigateToTileAction(target.currentTile, true));
                            }
                            else
                            {
                                EndCombatTurn();
                                return;
                            }
                        }
                    }
                    else
                    {
                        EndCombatTurn();
                    }
                }
                else
                {
                    EndCombatTurn();
                }
            }
            else
            {
                EndCombatTurn();
            }
        }
        
        if (character.isIdle && character.isAlive)
        {
            if (!obstacle.enabled)
                obstacle.enabled = true;
        }
	}

    public override void Loot(Container container)
    {
        // container.TakeAllItems(this);
    }
}

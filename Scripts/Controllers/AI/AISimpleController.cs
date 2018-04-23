using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AISimpleController : NeverdawnCharacterController
{

    private float delay;

    private NavMeshObstacle obstacle;

    private Character target;


    void Start()
    {
        character = GetComponent<Character>();
        obstacle = GetComponent<NavMeshObstacle>();
        color = GameSettings.neutralColor;
    }

	// Update is called once per frame
    public override void UpdateCombatControls()
    {

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
                    .OrderByDescending(m => m.threatRating).FirstOrDefault();

                if (meleeAbility != null)
                {
                    target = CharacterUtils.GetClosestCharacter(GameController.instance.party, character.position);

                    if (target != null)
                    {
                        float distance = Vector3.Distance(character.position, target.position);

                        if (distance > meleeAbility.attackRange)
                        {
                            if (character.remainingSteps > 0.0f)
                            {
                                character.PushAction(new CharacterNavigateToAction(target.position, character.remainingSteps, false,
                                    meleeAbility.attackRange - 0.1f));
                            }
                            else
                            {
                                EndCombatTurn();
                                return;
                            }
                        }
                        else
                        {
                            meleeAbility.caster = character;
                            meleeAbility.target = target.frame;

                            character.PushAction(meleeAbility.Prepare());
                            character.PushAction(meleeAbility.Cast());
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



        if (character.isIdle)
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

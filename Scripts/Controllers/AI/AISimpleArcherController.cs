using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AISimpleArcherController : NeverdawnCharacterController
{

    private float delay;

    private NavMeshObstacle obstacle;

    private Character target;
    private Vector3 fallbackPosition;
    private Vector3 fallBackVelocity;

    [SerializeField]
    private Transform launchTransform;
    private bool hasFallback;

    void Start()
    {
        character = GetComponent<Character>();
        obstacle = GetComponent<NavMeshObstacle>();
    }
    public override void UpdateCombatControls()
    {
        if (GameController.state == GameState.Combat)
        {
            if (character.isIdle)
            {
                if (obstacle.enabled)
                {
                    obstacle.enabled = false;
                    return;
                }

                if (character.remainingActions > 0)
                {
                    RangedAttackAbility rangedAttack = character.GetCastableAbilities<RangedAttackAbility>()
                        .OrderByDescending(m => m.threatRating).FirstOrDefault();

                    if (rangedAttack != null)
                    {
                        target = CharacterUtils.GetClosestCharacter(GameController.instance.party, character.position);

                        if (target != null)
                        {
                            NeverdawnCamera.AddTargetLerped(target.transform);
                            hasFallback = false;

                            float distance = Vector3.Distance(character.position, target.position);

                            if (distance > rangedAttack.weapon.estimatedRange + character.remainingSteps)
                            {
                                if (character.remainingSteps > 0.0f)
                                {
                                    character.PushAction(new CharacterNavigateToAction(target.position, character.remainingSteps, false,
                                        rangedAttack.weapon.estimatedRange - 0.1f));
                                }
                                else
                                {
                                    GameController.instance.combatController.EndTurn(this);
                                    return;
                                }
                            }
                            else
                            {
                                // Get positions and sort by strategic value (furthest away from target!)
                                List<Vector3> positions = sampleSurroundings(20, character.remainingSteps);
                                positions = positions.OrderByDescending(p => Vector3.Distance(p, target.position)).ToList();

                                // No position found yet!
                                bool hasPosition = false;
                                Vector3 targetPosition = Vector3.negativeInfinity;
                                Vector3 targetVelocity = Vector3.negativeInfinity;
                                Vector3 force;

                                foreach (Vector3 position in positions)
                                {
                                    Vector3 direction = target.position - position;
                                    direction.y = 0.0f;
                                    direction.Normalize();

                                    launchTransform.position = position;
                                    launchTransform.forward = direction;

                                    Vector3 launchPosition = launchTransform.TransformPoint(rangedAttack.weapon.projectileSpawn);

                                    if (tryHitTarget(rangedAttack, launchPosition, target.frame, out force, 5))
                                    {
                                        targetPosition = position;
                                        targetVelocity = force;
                                        hasPosition = true;
                                        break;
                                    }
                                }

                                if (hasPosition)
                                {
                                    rangedAttack.targetVelocity = targetVelocity;
                                    rangedAttack.caster = character;

                                    character.PushAction(new CharacterNavigateToAction(targetPosition, character.remainingSteps, false));
                                    character.PushAction(rangedAttack.Prepare());
                                    character.PushAction(rangedAttack.Cast());

                                }
                                else
                                {
                                    if (hasFallback)
                                    {
                                        rangedAttack.targetVelocity = fallBackVelocity;
                                        rangedAttack.caster = character;

                                        character.PushAction(new CharacterNavigateToAction(fallbackPosition, character.remainingSteps, false));
                                        character.PushAction(rangedAttack.Prepare());
                                        character.PushAction(rangedAttack.Cast());
                                    }
                                    else
                                    {
                                        GameController.instance.combatController.EndTurn(this);
                                    }
                                }
                            }
                        }
                        else
                        {
                            GameController.instance.combatController.EndTurn(this);
                        }
                    }
                    else
                    {
                        EndCombatTurn();
                    }
                }
                else
                {
                    if (target)
                    {
                        NeverdawnCamera.RemoveTargetLerped(target.transform);
                    }

                    EndCombatTurn();
                }
            }
        }


        if (character.isIdle)
        {
            if (!obstacle.enabled)
                obstacle.enabled = true;
        }
	}

    private bool isBetterPosition(Vector3 position, Vector3 targetPosition, Vector3 vector3)
    {
        return Vector3.Distance(position, vector3) > Vector3.Distance(targetPosition, vector3); 
    }

    private bool tryHitTarget(RangedAttackAbility rangedAttack, Vector3 position, Frame target, out Vector3 force, int steps)
    {
        float velocity = rangedAttack.weapon.range / 2.0f;

        float velocityStep = velocity / (float)steps;

        Vector3 direction = target.position - position;
        direction.y = 0.0f;

        float distance = direction.magnitude;

        direction.Normalize();

        for (int i = 0; i < steps + 1; i++)
        {
            if (ProjectileUtils.canHitCoordinate(distance, 0.0f, velocity))
            {
                float angle1 = ProjectileUtils.calculateAngle1ToHitCoordinate(distance, 0.0f, velocity);
                float angle2 = ProjectileUtils.calculateAngle2ToHitCoordinate(distance, 0.0f, velocity);

                if (ProjectileUtils.tryHitWithBallisticCast(target, position, velocity * direction, angle1, out force))
                {
                    return true;
                }

                if (ProjectileUtils.tryHitWithBallisticCast(target, position, velocity * direction, angle2, out force))
                {
                    return true;
                }

                if (!hasFallback)
                {
                    fallbackPosition = position;
                    fallBackVelocity = force;
                    hasFallback = true;
                }
            }

            velocity += velocityStep;
        }

        force = Vector3.negativeInfinity;
        return false;
    }


    private List<Vector3> sampleSurroundings(int count, float radius)
    {
        List<Vector3> result = new List<Vector3>();

        float stepWidth = (radius * 2.0f) / (float)count;
        int count2 = count / 2;

        for (int i = -count2; i < count2; i++)
        {
            for (int j = -count2; j < count2; j++)
            {
                Vector3 position = character.position;
                position.x += stepWidth * i;
                position.z += stepWidth * j;

                NavMeshHit hit;

                if(NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    position = hit.position;

                    if (Vector3.Distance(position, character.position) <= character.remainingSteps)
                    {
                        NavMeshPath path = new NavMeshPath();

                        if (NavMesh.CalculatePath(character.position, position, NavMesh.AllAreas, path))
                        {
                            if (NeverdawnUtility.GetPathLength(path) <= character.remainingSteps)
                            {
                                result.Add(position);
                            }
                        }
                    }
                }
            }
        }

        return result;
    }

    public override void Loot(Container container)
    {
        // container.TakeAllItems(this);
    }

   
}

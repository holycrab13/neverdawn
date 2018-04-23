using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Linq;


public class SingleTargetProjectileCursor : AbilityCursorBase
{
    private GameObject selector;

    [SerializeField]
    private float height;

    [SerializeField]
    private OnHitCursorTargetMode targetMode;

    [SerializeField]
    private float flashIntensity;

    /// <summary>
    /// Interaction with ability
    /// </summary>
    /// 
    public int maxRange { get; set; }

    public Vector3 projectileOffset { get; set; }

    /// <summary>
    /// Properties set in the cursor
    /// </summary>
    public Frame target { get; private set; }

    public bool targetSelf { get; private set; }

    public Vector3 targetVelocity { get; private set; }

    public float timeOfFlight { get; private set; }

    private Frame[] targets;

    private int targetIndex;

    private Material material;

    private Color selectorColor;

    private float time;

    private float firingAngle;

    private List<HexTile> tilesInRange;

    private int outlineHandle;
    
    /// <summary>
    /// Init the cursor
    /// </summary>
    protected override void OnInitializeCursor()
    {
        selector = HexTerrain.CreateTileSelector(controller.color, 1.5f, 0.9f);
        outlineHandle = HexTerrain.CreateBorder(HexTerrain.GetTilesInRange(character.currentTile, maxRange, true), controller.color);

        AlphaPingPong anim = selector.AddComponent<AlphaPingPong>();
        anim.intensity = 0.5f;

        selector.transform.position = Vector3.forward * 10000.0f;

        if (GameController.state == GameState.Exploration)
        {
            HexCamera.ShowMask(character, maxRange);
        }
        
        updateTargets();
        updateSelector();
    }

    /// <summary>
    /// Fetch the target array for this cursor. This highly depends on the target mode
    /// </summary>
    private void updateTargets()
    {
        HexTile tile = character.currentTile;

        tilesInRange = HexTerrain.GetTilesInRange(tile, maxRange, false);

        targets = tilesInRange.Where(t => t.currentFrame)
            .Select(t => t.currentFrame)
            .Where(t => isValidTarget(t))
            .ToArray();
    }

    /// <summary>
    /// Checks whether target t is a valid target
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private bool isValidTarget(Frame t)
    {
        return !GameController.instance.party.IsCharacterInParty(t) && t.GetComponent<Destructible>() && tryHitTarget(t);
    }

    /// <summary>
    /// Updates the cursor with joystick info
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="intensity"></param>
    public override void UpdateValues(Vector3 direction, float intensity)
    {
        if (targets.Length == 0)
            return;

        // Check if current target is still valid
        if (Vector3.Distance(character.transform.position, targets[targetIndex].position) > maxRange)
        {
            targetIndex = NeverdawnUtility.RepeatIndex(targetIndex, targets.Length);
        }
    }

    /// <summary>
    /// Updates the cursor with previous button press
    /// </summary>
    public override void Previous()
    {
        updateTargets();
        targetIndex = NeverdawnUtility.RepeatIndex(targetIndex + 1, targets.Length);

        updateSelector();
    }

    /// <summary>
    /// Updates the cursor with next button press
    /// </summary>
    public override void Next()
    {
        updateTargets();
        targetIndex = NeverdawnUtility.RepeatIndex(targetIndex - 1, targets.Length);

        updateSelector();

    }

    private void updateSelector()
    {
        if (targets.Length > 0)
        {
            selector.transform.position = targets[targetIndex].currentTile.position + Vector3.up * height; Vector3 direction = targets[targetIndex].position - character.position;
            direction.y = 0.0f;
            direction.Normalize();

            character.PushAction(new CharacterTurnAction(direction));
        }
        else
        {
            selector.transform.position = Vector3.forward * 10000.0f;
        }
    }

    /// <summary>
    /// Cancel the cursor if the target UI is null, close it otherwise
    /// </summary>
    /// <returns></returns>
    protected override bool OnCancelCursor()
    {
        return true;
    }


    protected override void OnHideCursor()
    {
        HexTerrain.DestroyBorder(outlineHandle);
        HexCamera.HideMask(character);
        selector.gameObject.SetActive(false);
    }

    /// <summary>
    /// Confirm the cursor
    /// </summary>
    /// <returns></returns>
    protected override bool OnConfirmCursor()
    {
        if (targets.Length > 0 && targets[targetIndex] != null)
        {
            // We have an unambigous selection
            targetSelf = targets[targetIndex] == character;
            target = targets[targetIndex];

            Vector3 position = character.transform.position + character.transform.TransformDirection(projectileOffset);
            Vector3 targetPosition = target.GetComponentInChildren<Collider>().bounds.center;

            targetVelocity = getVelocityForTarget(position, targetPosition, 45.0f);
            return true;
        }

        // return false if there is no target at all
        return false;
    }

    private bool tryHitTarget(Frame t)
    {
        return true;
    }


    private Vector3 getVelocityForTarget(Vector3 position, Vector3 target, float initialAngle)
    {
        float gravity = GameSettings.gravity;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPostion = new Vector3(position.x, 0, position.z);


        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = position.y - target.y;

        float speed = ProjectileUtils.LaunchSpeed(distance, yOffset, gravity, angle);
        Vector3 velocity = new Vector3(0, speed * Mathf.Sin(angle), speed * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.SignedAngle(Vector3.forward, planarTarget - planarPostion, Vector3.up);

        timeOfFlight = ProjectileUtils.TimeOfFlight(speed, angle, yOffset, gravity);
        return Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
    }
}

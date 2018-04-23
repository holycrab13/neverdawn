using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Linq;

public enum OnHitCursorTargetMode
{
    Enemy,
    All,
    Allies,
    Party
}


public class SingleTargetCursor : AbilityCursorBase
{
    [SerializeField]
    private UIQuickMenuPickTarget resolveTargetUIPrefab;

    [SerializeField]
    private float height;

    [SerializeField]
    private OnHitCursorTargetMode targetMode;

    private Frame[] targets;

    private int targetIndex;

    private Material material;

    private Color selectorColor;

    private float time;

    private UIQuickMenuPickTarget resolveTargetUI;
    private GameObject selector;

    /// <summary>
    /// Init the cursor
    /// </summary>
    protected override void OnInitializeCursor()
    {
        selector = HexTerrain.CreateTileSelector(controller.color, 1.5f, 0.9f);

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

        List<HexTile> tilesInRange = HexTerrain.GetTilesInRange(tile, maxRange, false);
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
        return !GameController.instance.party.IsCharacterInParty(t) && t.GetComponent<Destructible>();
    }

    /// <summary>
    /// Updates the cursor
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="intensity"></param>
    public override void UpdateValues(Vector3 direction, float intensity)
    {
        // TODO: HIDE SELECTOR AND DO THIS PROPERLY
        if (targets.Length == 0)
            return;

        // Check if current target is still valid
        if (Vector3.Distance(character.transform.position, targets[targetIndex].position) > maxRange)
        {
            updateTargets();
            targetIndex = NeverdawnUtility.RepeatIndex(targetIndex, targets.Length);
        }
    }

    public override void Previous()
    {
        updateTargets();
        targetIndex = NeverdawnUtility.RepeatIndex(targetIndex + 1, targets.Length);

        updateSelector();
    }

    public override void Next()
    {
        updateTargets();
        targetIndex = NeverdawnUtility.RepeatIndex(targetIndex - 1, targets.Length);

        updateSelector();

    }

    private void updateSelector()
    {
        if (targets.Length > 0 && resolveTargetUI == null)
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
            // Is there any ambivalency? Self select with hidden characters
            if (targets[targetIndex] == character && GameController.instance.party.hiddenCharacters.Count > 0)
            {
                // already resolving?
                if (resolveTargetUI != null)
                {
                    // resolving with result?
                    if (resolveTargetUI.selected != null)
                    {
                        target = resolveTargetUI.selected.frame;
                        targetSelf = true;
                        controller.characterMenu.Close();
                        return true;
                    }
                    else
                    {
                        controller.characterMenu.Close();
                        return false;
                    }
                }
                else
                {
                    List<Character> possibleTargets = new List<Character>();
                    possibleTargets.Add(character);
                    possibleTargets.AddRange(GameController.instance.party.hiddenCharacters);

                    resolveTargetUI = Instantiate(resolveTargetUIPrefab);
                    resolveTargetUI.possibleTargets = possibleTargets.ToArray();
                    controller.characterMenu.Open(resolveTargetUI);
                    return false;
                }
            }
            else
            {
                // We have an unambigous selection
                targetSelf = targets[targetIndex] == character;
                target = targets[targetIndex];
                return true;
            }
        }

        // return false if there is no target at all
        return false;
    }

    public int maxRange { get; set; }

    public Frame target { get; private set; }

    public bool targetSelf { get; private set; }



}

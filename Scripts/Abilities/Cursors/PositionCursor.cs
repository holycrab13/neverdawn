using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Linq;


public class PositionCursor : AbilityCursorBase
{
    [SerializeField]
    private UIQuickMenuPickTarget resolveTargetUIPrefab;

    [SerializeField]
    private Transform selector;

    [SerializeField]
    private float height;

    [SerializeField]
    private OnHitCursorTargetMode targetMode;

    private List<Character> targets;

    private int targetIndex;

    private Material material;

    private Color selectorColor;

    private float time;

    private UIQuickMenuPickTarget resolveTargetUI;
  
    /// <summary>
    /// Init the cursor
    /// </summary>
    protected override void OnInitializeCursor()
    {
        material = selector.GetComponent<Projector>().material;
        selectorColor = color;

        fetchTargets();
    }

    /// <summary>
    /// Fetch the target array for this cursor. This highly depends on the target mode
    /// </summary>
    private void fetchTargets()
    {
        targets = new List<Character>();

        if (targetMode == OnHitCursorTargetMode.Enemy)
        {
            targets = CharacterUtils.GetCharactersInRange(GameController.instance.enemies, character.transform.position, maxRange);
        }

        if (targetMode == OnHitCursorTargetMode.Allies)
        {
            targets = CharacterUtils.GetCharactersInRange(GameController.instance.party.activeCharacters, character.transform.position, maxRange);
        }
    }

    /// <summary>
    /// Updates the cursor
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="intensity"></param>
    public override void UpdateValues(Vector3 direction, float intensity)
    {
        // TODO: HIDE SELECTOR AND DO THIS PROPERLY
        if (targets.Count == 0)
            return;

        // Check if current target is still valid
        if (Vector3.Distance(character.transform.position, targets[targetIndex].position) > maxRange)
        {
            fetchTargets();
            targetIndex = NeverdawnUtility.RepeatIndex(targetIndex, targets.Count);
        }

        updateSelector();
    }

    private void updateSelector()
    {
        time += Time.deltaTime / 2.0f;
        selectorColor.a = 0.5f + Mathf.PingPong(time, 0.5f);
        material.color = selectorColor;

        if (targets.Count > 0 && resolveTargetUI == null)
        {
            selector.transform.position = targets[targetIndex].position + Vector3.up * height;
        }
        else
        {
            selector.transform.position = Vector3.up * 1000.0f;
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

    /// <summary>
    /// Confirm the cursor
    /// </summary>
    /// <returns></returns>
    protected override bool OnConfirmCursor()
    {
        if (targets.Count > 0 && targets[targetIndex] != null)
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
                        target = resolveTargetUI.selected;
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

    public float maxRange { get; set; }

    public Character target { get; private set; }

    public bool targetSelf { get; private set; }



    protected override void OnHideCursor()
    {
       
    }
}

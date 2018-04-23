using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class CharacterDrawCombatItemAction : CharacterActionBase, IExitStateListener
{
    private CombatItem combatItem;


    private CharacterSheathCombatItemAction sheatheAction;
    private bool once;


    private AnimationClip animationClips;

    private Animator animator;

    private AbilityBase ability;

    public CharacterDrawCombatItemAction(CombatItem item, AbilityBase ability = null)
    {
        this.combatItem = item;
        this.ability = ability;
    }

    protected override void OnCharacterActionStart(Character character)
    {
        if(character.activeCombatItem == combatItem)
        {
            applyOverrides();
            Done();
            return;
        }

     
        if (character.activeCombatItem != null)
        {
            sheatheAction = new CharacterSheathCombatItemAction();
            sheatheAction.ActionStart(character);
        }
        else
        {
            draw();
        }
    }

    public override void ActionUpdate(float timekey)
    {
        if (sheatheAction != null)
        {
            if (!sheatheAction.IsDone)
            {
                sheatheAction.ActionUpdate(timekey);
            }

            if (sheatheAction.IsDone && !once)
            {
                once = true;

                draw();
            }
        }

        if (IsCancelled)
            Done();
    }

    private void draw()
    {
        applyOverrides();

        Mannequin mannequin = character.GetComponent<Mannequin>();

        if (mannequin != null)
        {
            mannequin.ShowItemInHandsView(null);
        }

        Animator animator = character.animator;

        character.activeCombatItem = combatItem;

        if (combatItem.animationOverrides.equip == null)
        {
            animator.SetTrigger("ToCombatStance");
            ExitAlertBehaviour.AlertExitState(animator, "Grounded", this);
        }
        else
        {
            ExitAlertBehaviour.AlertExitState(animator, "DrawWeapon", this);
            animator.SetTrigger("DrawWeapon");
        }

    }

    private void applyOverrides()
    {
        AnimatorOverrideController controller = character.animatorOverrideController;

        AnimationClipMap overrides = combatItem.animationOverrides.GetOverrides(controller);

        if (ability != null && ability.animation != null)
            overrides["Default Attack"] = ability.animation;

        controller.ApplyOverrides(overrides);
    }

    public void OnExitState()
    {
        Done();
    }
}




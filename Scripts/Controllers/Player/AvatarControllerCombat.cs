using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class AvatarController : NeverdawnCharacterController
{
    private UIQuickMenuCombat combatPage;

    public override void StartCombatTurn()
    {
        base.StartCombatTurn();

        combatPage = Instantiate(UIFactory.uiQuickMenuCombatPrefab);
        combatPage.character = character;
        combatPage.includeWalk = true;
        combatPage.includeDoNothing = true;
        characterMenu.Open(combatPage);

        AbilityBase walkAbility = Instantiate(CombatController.walkAbility);
        walkAbility.Initialize(character.gameObject);

        UIQuickMenuCastAbility instance = Instantiate(UIFactory.uiQuickMenuCastAbilityPrefab);
        instance.ability = walkAbility;
        characterMenu.NavigateInto(instance);

        CastAbility(walkAbility);
    }

    public override void UpdateCombatControls()
    {
        base.UpdateCombatControls();

        if (characterMenu.isOpen && characterMenu.currentPage != combatPage)
        {
            if (inputModule.GetButtonDown(NeverdawnInputButton.Cancel))
            {
                characterMenu.GoBack();
            }
        }

        // disable interactions
        updateInteraction(null);

        if (currentAbility == null)
        {

            if (character.remainingActions <= 0)
            {
 
                characterMenu.Close();
                EndCombatTurn();
                return;
            }
        }
        else
        {
            updateAbility();
            return;
        }
    }

}

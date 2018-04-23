using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class AvatarController : NeverdawnCharacterController
{
    /// <summary>
    /// Ability to cast
    /// </summary>
    private AbilityBase currentAbility;

    private bool cursorConfirmed;

  

    /// <summary>
    /// Update the picking process
    /// </summary>
    private void updateAbility()
    {
        if (!currentAbility.cursor.isActive && character.isIdle)
        {
            cursorConfirmed = false;

            character.PushAction(currentAbility.Relax());
            currentAbility.DestroyCursor();
            currentAbility = null;
            return;
        }

        if (currentAbility.cursor != null && currentAbility.cursor.isActive)
        {
            currentAbility.cursor.UpdateValues(inputModule.normalizedDirection * Time.deltaTime * 5.0f,
                inputModule.GetAxis(NeverdawnInputAxis.VerticalRight) * Time.deltaTime);
            
            if (inputModule.GetButtonDown(NeverdawnInputButton.Left))
            {
                currentAbility.cursor.Previous();
            }

            if (inputModule.GetButtonDown(NeverdawnInputButton.Right))
            {
                currentAbility.cursor.Next();
            }

            if (inputModule.GetButtonDown(NeverdawnInputButton.Confirm))
            {
                if (currentAbility.cursor.Confirm())
                {
                    currentAbility.ApplyCursor();
                    character.PushAction(currentAbility.Cast());
                }
            }

            if (inputModule.GetButtonDown(NeverdawnInputButton.Cancel))
            {
                if (currentAbility.cursor.Cancel())
                {
                    character.PushAction(currentAbility.Relax());
                }
            }
        }
    }
}

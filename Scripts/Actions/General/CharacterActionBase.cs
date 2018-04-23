using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public abstract class CharacterActionBase : ActionBase
{
    protected Character character;

    public void ActionStart(Character actor)
    {
        this.character = actor;
        ActionStart();
    }

    protected override void OnActionStart()
    {
        if (character == null)
            throw new InvalidOperationException("No actor set on action. Use ActionStart(character).");

        OnCharacterActionStart(character);
    }

    protected abstract void OnCharacterActionStart(Character actor);
}

public class CharacterActionOnce
{
    private bool value = true;

    public bool Once()
    {
        if (value)
        {
            value = false;
            return true;
        }

        return false;
    }
}




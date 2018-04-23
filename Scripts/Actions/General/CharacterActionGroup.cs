using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class CharacterActionGroup : CharacterActionBase
{
    public Queue<CharacterActionBase> actions;

    private CharacterActionBase currentAction;

    public CharacterActionGroup()
    {
        actions = new Queue<CharacterActionBase>();
    }

    public void PushAction(CharacterActionBase action)
    {
        actions.Enqueue(action);
    }

    protected override void OnCharacterActionStart(Character actor)
    {

    }

    public override void ActionUpdate(float timekey)
    {
        if (currentAction != null)
        {
            if (currentAction.IsDone)
            {
                currentAction = null;
            }
            else
            {
                currentAction.ActionUpdate(timekey);
            }
        }


        if (currentAction == null)
        {
            if (actions.Count > 0)
            {
                currentAction = actions.Dequeue();
                currentAction.ActionStart(character);
            }
            else
            {
                Done();
            }
        }
    }
}





using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class InteractionBase : ScriptableObject
{
    public Sprite icon;

    public string label;

    public string description;

    public int priority;

    public AudioClip sound;

    public Func<string> descriptor;

    public Func<Character, bool> condition;

    public virtual void Interact(NeverdawnCharacterController character)
    {

    }

    public virtual bool IsAvailable(Character character)
    {
        if (condition != null)
        {
            return condition(character);
        }

        return true;
    }

    public virtual string GetLabel()
    {
        return label;
    }

    public virtual Sprite GetIcon()
    {
        return icon;
    }

    public virtual int GetPriority()
    {
        return priority;
    }

    public virtual string GetDescription()
    {
        if(descriptor != null)
        {
            return descriptor();
        }

        return description;
    }
}

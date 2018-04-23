using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class BuffBase : ScriptableObject
{
    public string description;

    public abstract bool IsDone { get; }

    public virtual bool Initialize(GameObject gameObject)
    {
        return true;
    }

    public abstract void UpdateBuff(Character neverdawnCharacter, float timekey);

    public virtual string GenerateDescription()
    {
        return description;
    }
}

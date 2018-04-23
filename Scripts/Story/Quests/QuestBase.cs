using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum QuestState
{
    Unknown,
    Accepted,
    Complete
}

public abstract class QuestBase : ScriptableObject
{
    public string label;

    public string description;

    public int experience;

    public int silver;

    public Pickable[] rewards;

    public Discovery[] prerequisites;

    protected void Complete()
    {

    }

    public void HandIn()
    {
        // claim reward stuff
    }

    public void Accept()
    {

    }

    public bool IsAvailable()
    {
        // see if matches prerequisites

        return true;
    }
}

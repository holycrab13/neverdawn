using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase
{
    private bool isDone;

    private bool isStarted;

    private bool isCancelled;

    public void ActionStart()
    {
        OnActionStart();
        isStarted = true;
    }

    public virtual void ActionConfirm()
    {

    }

    public void ActionCancel()
    {
        isCancelled = true;
    }

    protected void Done()
    {
        isDone = true;
    }

    protected abstract void OnActionStart();

    public abstract void ActionUpdate(float timekey);

    public bool IsDone
    {
        get { return isDone; }
    }

    public bool IsStarted
    {
        get { return isStarted; }
    }

    protected bool IsCancelled
    {
        get { return isCancelled; }
    }   
}

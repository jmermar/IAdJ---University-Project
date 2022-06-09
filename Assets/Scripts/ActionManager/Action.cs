using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    private string actionName;
    
    public string ActionName {
        get => actionName;
        set => actionName = value;
    }

    public abstract int GetPriority();
    public abstract int GetExpiryTime();

    public abstract bool CanInterrupt();
    public abstract bool CanDoBoth(Action other);
    public abstract bool IsComplete();

    public abstract void Execute(AgentNPC agent);
}

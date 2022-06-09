using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionManager : MonoBehaviour
{
    private List<Action> queue;
    private List<Action> active;
    private float timeCounter;
    private AgentNPC agent;

    void Awake() {
        timeCounter = 0;
        queue = new List<Action>();
        active = new List<Action>();
        agent = GetComponent<AgentNPC>();
    }

    public void ScheludeAction(Action a) {
        queue.Add(a);
    }

    private int GetActiveHighestPriority() {
        int max = -1;
        foreach(Action a in active) {
            max = Math.Max(a.GetPriority(), max);
        }

        return max;
    }

    void Update() {
        timeCounter += Time.deltaTime;
        foreach(Action a in queue) {
            if (a.GetPriority() < GetActiveHighestPriority()) continue;
            if (a.CanInterrupt()) {
                active.Clear();
                active.Add(a);
            }
        }

        List<Action> copy = new List<Action>(queue);

        foreach(Action a in copy) {
            if (a.GetExpiryTime() < timeCounter) {
                queue.Remove(a);
                continue;
            }

            bool add = true;
            foreach(Action a2 in active) {
                if (!a.CanDoBoth(a2)) {
                    add = false;
                    break;
                }
            }

            if (add) {
                active.Add(a);
                queue.Remove(a);
            }
        }

        copy = new List<Action>(active);
        foreach(Action a in copy) {
            if (a.IsComplete()) {
                active.Remove(a);
                continue;
            }

            a.Execute(agent);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRTASteering : SteeringBehavior
{
    private Seek seek;
    private Arrive arrive;

    private LRTA.LRTA lrta;
    private TargetAgent goal;
    private TargetAgent nextMove;
    private bool finalMove;

    [SerializeField] float arrivalRadius = 0.5f;

    public float ArrivalRadius { get => arrivalRadius; set => arrivalRadius = value; }

    protected override void Awake()
    {
        base.Awake();
        seek = gameObject.AddComponent<Seek>();
        arrive = gameObject.AddComponent<Arrive>();

        seek.Standalone = false;
        arrive.Standalone = false;

        goal = TargetAgent.CreateTarget();
        nextMove = TargetAgent.CreateTarget();
        seek.Target = nextMove;
        arrive.Target = goal;
    }

    public bool IsActive() => lrta != null;

    public void MoveTo(Vector3 position) {
        lrta = new LRTA.LRTA(Agent.Map, Agent.Position, position, true);
        Vector3 nextMove = Vector3.zero;
        bool end;
        if (lrta.GetNextMove(out nextMove, out end)) {
            goal.Position = position;
            if (end) {
                finalMove = true;    
            } else {
                this.nextMove.Position = nextMove;
                finalMove = false;
            }
        }
    }

    public void CancelMovement() {
        lrta = null;
    }

    public override Steering GetSteering(Agent agent)
    {
        if (lrta == null) {
            return null;
        } else {
            if (!finalMove) {
                if ((agent.Position - nextMove.Position).magnitude <= arrivalRadius) {
                    Vector3 nextMove = Vector3.zero;
                    bool end;
                    if (lrta.GetNextMove(out nextMove, out end)) {
                        if (end) {
                            finalMove = true;    
                        } else {
                            this.nextMove.Position = nextMove;
                            finalMove = false;
                            return seek.GetSteering(agent);
                        }
                    } else {
                        lrta = null;
                        return null;
                    }
                } else {
                    return seek.GetSteering(agent);
                }
            }

            if (finalMove) {
                Steering s = arrive.GetSteering(agent);
                if (s != null) {
                    return s;
                } else {
                    lrta = null;
                    Agent.ReachedGoal();
                }
            }

            return null;
        }
    }
}

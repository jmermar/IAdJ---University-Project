using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarSteering : SteeringBehavior
{
    private PathFollowing pathFollowing;
    private Arrive arrive;

    private Vector3[] points;
    private bool move;
    private bool justArrive = false;

    protected override void Awake() {
        base.Awake();
        Group = GroupSteering.Move;
        pathFollowing = gameObject.AddComponent<PathFollowing>();
        pathFollowing.Standalone = false;
        arrive = gameObject.AddComponent<Arrive>();
        arrive.Target = TargetAgent.CreateTarget();
        arrive.Standalone = false;
        move = false;
    }

    public bool IsActive() => move;

    public bool MoveTo(Vector3 target) {
        Vector3[] points = AStar.GetPath(Agent.Position, target, Agent.Map);
        if (points != null && points.Length > 1) {
            this.points = points.Take(points.Length - 1).ToArray();
            target = points[points.Length - 1];
            move = true;
            arrive.Target.Position = target;

            pathFollowing.Points = points;
            pathFollowing.CurrentPoint = 0;
            pathFollowing.OnGoal = false;

            justArrive = false;
            arrive.Target.Position = target;
            return true;
        } else if (points != null) {
            justArrive = true;
            arrive.Target.Position = target;
            return true;
        } else {
            Debug.Log("No se pudo encontrar una ruta");
            return false;
        }
    }

    public void CancelMovement() {
        move = false;
    }

    public override Steering GetSteering(Agent agent)
    {
        if (!move) return null;

        if (!justArrive) {
            if (!pathFollowing.OnGoal) {
                return pathFollowing.GetSteering(agent);
            } else {
                justArrive = true;
            }
        }else {
            Steering steer = arrive.GetSteering(agent);
            if (steer == null) {
                move = false;
                Agent.ReachedGoal();
            }
            return steer;
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationSteering : SteeringBehavior
{
    private Formation formation;

    private Arrive arrive;
    private Align align;

    protected override void Awake()
    {
        base.Awake();

        Target = TargetAgent.CreateTarget();

        arrive = gameObject.AddComponent<Arrive>();
        arrive.Standalone = false;
        arrive.Target = Target;
        arrive.TimeToTarget = 0.25f;

        align = gameObject.AddComponent<Align>();
        align.TimeToTarget = 0.1f;
        align.Target = Target;
        align.Standalone = false;
    }

    public void EnterFormation(Formation f) {
        formation = f;
    }

    public void ExitFormation(Formation f) {
        formation = null;
    }

    public override Steering GetSteering(Agent agent)
    {
        if (formation == null) return null;
        if (formation.Estado == Formation.Status.InFormation) {
            Static target = formation.GetSlotPosition(Agent);
            Target.Position = target.position;
            Target.Orientation = target.orientation;
            Steering steer = arrive.GetSteering(agent);
            if (steer == null) {
                steer = align.GetSteering(agent);
            }

            return steer;
        }

        return null;
    }
}

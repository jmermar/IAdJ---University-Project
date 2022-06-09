using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereYouGoing : Align
{
    [SerializeField] private float epsilon = 0.2f;

    public float Epsilon { get => epsilon; set => epsilon = value; }

    void Start()
    {
    }

    protected override void Awake() {
        base.Awake();
        Target = TargetAgent.CreateTarget();
    }

    public override Steering GetSteering(Agent agent)
    {
        if (agent.Speed < Epsilon) return null;
        Target.Orientation = Mathf.Atan2(agent.Velocity.x, agent.Velocity.z) * Mathf.Rad2Deg;

        return base.GetSteering(agent);
    }
}

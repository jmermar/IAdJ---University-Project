using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Face
{
    [SerializeField] protected float wanderOffset = 3;
    [SerializeField] protected float wanderRadius = 0.1f;
    [SerializeField] protected float wanderRate = 30;
    [SerializeField] protected float wanderOrientation = 0;

    protected float WanderOffset { get => wanderOffset; set => wanderOffset = value; }
    protected float WanderRadius { get => wanderRadius; set => wanderRadius = value; }
    protected float WanderRate { get => wanderRate; set => wanderRate = value; }
    protected float WanderOrientation { get => wanderOrientation; set => wanderOrientation = value; }

    void Start()
    {
    }

    protected override void Awake() {
        base.Awake();
        ActualTarget = TargetAgent.CreateTarget();
    }

    public override Steering GetSteering(Agent agent)
    {
        wanderOrientation += Random.Range(-1f, 1f) * wanderRate;
        ActualTarget.Orientation = wanderOrientation + agent.Orientation;
        ActualTarget.Position = agent.Position + agent.OrientationToVector() * wanderOffset;
        ActualTarget.Position += ActualTarget.OrientationToVector() * wanderRadius;

        Steering steer = base.GetSteering(agent);
        if (steer == null) steer = new Steering();
        steer.linear = agent.OrientationToVector() * agent.MaxAcceleration;

        return steer;
    }
}

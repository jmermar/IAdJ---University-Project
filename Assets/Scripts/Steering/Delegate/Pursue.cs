using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek
{
    [SerializeField] protected float _maxPrediction = 1;

    [SerializeField] protected Agent _actualTarget;

    public float MaxPrediction {
        get => _maxPrediction;
        set => _maxPrediction = Mathf.Max(0.0000000001f, value);
    }

    public Agent ActualTarget {
        get => _actualTarget;
        set => _actualTarget = value;
    }

    protected override void Awake() {
        base.Awake();
        Target = TargetAgent.CreateTarget();
    }

    void Start()
    {
    }

    public override Steering GetSteering(Agent agent)
    {
        Vector3 direction = ActualTarget.Position - agent.Position;
        direction.y = 0;
        float distance = direction.magnitude;

        float speed = agent.Speed;

        float prediction;
        if (speed < distance / MaxPrediction) {
            prediction = MaxPrediction;
        } else {
            prediction = distance / speed;
        }

        Target.CopyFrom(Target);
        Target.Position += Target.Velocity * prediction;
        return base.GetSteering(agent);
    }
}

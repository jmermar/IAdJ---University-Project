using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    [SerializeField] protected Agent _actualTarget;

    public Agent ActualTarget {
        get => _actualTarget;
        set => _actualTarget = value;
    }

    void Start()
    {
    }

    protected override void Awake() {
        base.Awake();
        base.Target = TargetAgent.CreateTarget();
    }

    public override Steering GetSteering(Agent agent)
    {
        Vector3 direction = ActualTarget.Position - agent.Position;
        if (direction.magnitude < 0.00001f) return null;

        Target.CopyFrom(agent);
        Target.Orientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        return base.GetSteering(agent);
    }
}

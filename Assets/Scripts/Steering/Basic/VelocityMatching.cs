using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatching : SteeringBehavior
{
    [SerializeField] protected Agent _target;
    [SerializeField] protected float _timeToTarget = 0.001f;

    public float TimeToTarget {
        get => _timeToTarget;
        set => _timeToTarget = Mathf.Max(0, value);
    }
    void Start()
    {
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        steer.linear = Target.Velocity - agent.Velocity;
        steer.angular = 0;

        steer.linear /= TimeToTarget;

        if (steer.linear.magnitude > agent.MaxAcceleration) {
            steer.linear /= steer.linear.magnitude;
            steer.linear *= agent.MaxAcceleration;
        }

        // Retornamos el resultado final.
        return steer;
    }
}
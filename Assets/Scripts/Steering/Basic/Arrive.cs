using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehavior
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

        Vector3 delta = Target.Position - agent.Position;
        delta.y = 0;

        float distance = delta.magnitude;
        float targetSpeed = 0;
        if (distance < agent.InteriorRadius) {
            return null;
        }

        if (distance > agent.ArrivalRadius) targetSpeed = agent.MaxSpeed;
        else targetSpeed = agent.MaxSpeed * distance / agent.ArrivalRadius;

        Vector3 targetVelocity = delta.normalized * targetSpeed;

        steer.linear = (targetVelocity - agent.Velocity) / TimeToTarget;

        if (steer.linear.magnitude > agent.MaxAcceleration) steer.linear = steer.linear.normalized * agent.MaxAcceleration;

        steer.angular = 0;
        return steer;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek_IanMillington : SteeringBehavior
{
    [SerializeField] protected Agent _target;

    void Start()
    {
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        Vector3 delta = Target.Position - agent.Position;
        delta.y = 0;


        Vector3 desiredVelocity = delta.normalized * agent.MaxSpeed;

        steer.linear = desiredVelocity - agent.Velocity;
        steer.angular = 0;

        if (delta.magnitude < agent.InteriorRadius) steer.linear = Vector3.zero;


        // Retornamos el resultado final.
        return steer;
    }
}

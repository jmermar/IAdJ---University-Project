using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehavior
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

        steer.linear = delta.normalized * agent.MaxAcceleration;
        steer.angular = 0;

        // Retornamos el resultado final.
        return steer;
    }
}
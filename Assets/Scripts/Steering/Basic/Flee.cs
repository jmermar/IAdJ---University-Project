using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehavior
{
    void Start()
    {
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        Vector3 delta = agent.Position - Target.Position;
        delta.y = 0;

        steer.linear = delta.normalized * agent.MaxAcceleration;
        steer.angular = 0;

        return steer;
    }
}
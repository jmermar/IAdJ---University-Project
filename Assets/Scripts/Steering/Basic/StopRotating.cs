using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotating : SteeringBehavior
{
    [SerializeField] private float _predict = 0.05f;

    public float Predict { get => _predict; set => _predict = value; }

    void Start()
    {
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        steer.angular = -Mathf.Sign(agent.Rotation) * agent.MaxAngularAcc;
        if (Mathf.Abs(agent.Rotation) - Mathf.Abs(steer.angular) * _predict < 0) {
            steer.angular = Mathf.Sign(steer.angular) * Mathf.Abs(agent.Rotation) / _predict;
        }

        steer.linear = Vector3.zero;

        return steer;
    }
}

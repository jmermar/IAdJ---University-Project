using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehavior
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

        steer.linear = Vector3.zero;


        float rotation = Bodi.SignedAngle(Target.Orientation, agent.Orientation);
        float rotationSize = Mathf.Abs(rotation);
        float sign = rotation / rotationSize;

        if (rotationSize < agent.InteriorAngle) {
            return null;
        }

        float targetRotation;
        if (rotationSize > agent.ExteriorAngle) targetRotation = sign * agent.MaxAngularAcc;
        else targetRotation = sign * agent.MaxRotation * rotationSize / agent.ExteriorAngle ;

        steer.angular = (targetRotation - agent.Rotation) / TimeToTarget;

        float angularAcceleration = Mathf.Abs(steer.angular);
        if (steer.angular > agent.MaxAngularAcc) {
            steer.angular /= angularAcceleration;
            steer.angular *= agent.MaxAngularAcc;
        }

        // Retornamos el resultado final.
        return steer;
    }
}
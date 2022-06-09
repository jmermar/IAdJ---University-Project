using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bodi : MonoBehaviour
{
    [SerializeField] protected float _mass = 1;
    [SerializeField] protected float _maxSpeed = 1;
    [SerializeField] protected float _maxRotation = 1;
    [SerializeField] protected float _maxAcceleration = 1;
    [SerializeField] protected float _maxAngularAcc = 1;
    [SerializeField] protected float _maxForce = 1;

    protected Vector3 _acceleration; // aceleración lineal
    protected float _angularAcc;  // aceleración angular
    protected Vector3 _velocity; // velocidad lineal
    protected float _rotation;  // velocidad angular
    protected float _orientation;  // 'posición' angular


    /// <summary>
    /// Mass for the NPC
    /// </summary>
    public float Mass
    {
        get { return _mass; }
        set { _mass = value; }
    }

    public float MaxForce {
        get => _maxForce;
        set => _maxForce = value;
    }
    // public float MaxSpeed
    public float MaxSpeed {
        get => _maxSpeed;
        set => _maxSpeed = Mathf.Max(0, value);
    }
    // public Vector3 Velocity
    public Vector3 Velocity {
        get => _velocity;
        set {
            _velocity = value;
            Speed = _velocity.magnitude;
        }
    }
    // public float MaxRotation
    public float MaxRotation {
        get => _maxRotation;
        set => _maxRotation = Mathf.Max(0, value);
    }
    // public float Rotation. 
    public float Rotation {
        get => _rotation;
        set => _rotation = Mathf.Min(MaxRotation, Mathf.Max(-MaxRotation, value));
    }
    // public float MaxAcceleration
    public float MaxAcceleration {
        get => _maxAcceleration;
        set => _maxAcceleration = Mathf.Max(0, value);
    }
    // public float MaxAcceleration
    public float MaxAngularAcc {
        get => _maxAngularAcc;
        set => _maxAngularAcc = value;
    }
    // public Vector3 Acceleration
    public Vector3 Acceleration {
        get => _acceleration;
        set {
            _acceleration = value;
            if (_acceleration.magnitude > MaxAcceleration) _acceleration = _acceleration.normalized * MaxAcceleration;
        }
    }
    // public float AngularAcc
    public float AngularAcc {
        get => _angularAcc;
        set => _angularAcc = value;
    }
    // public Vector3 Position. Recuerda. Esta es la única propiedad que trabaja sobre transform.
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    // public float Orientation
    public float Orientation {
        get => _orientation;
        set => _orientation = WrapAngle(value);
    }
    // public float SpeED
    public float Speed {
        get => _velocity.magnitude;
        set => _velocity = _velocity.normalized * Mathf.Max(0, Mathf.Min(MaxSpeed, value));
    }

    // TE PUEDEN INTERESAR LOS SIGUIENTES MÉTODOS.
    // Quita o añade todos los que sean referentes a la parte física.

    public static float WrapAngle(float angle) {
        float wrapped = Mathf.Repeat(angle, 360);
        return (wrapped == 360) ? 0 : wrapped;
    }

    // public float Heading()
    public static float SignedAngle(float angle1, float angle2)
    {
        float angle = angle1 - angle2;
        return WrapAngle(angle + 180) - 180;
    }
    // public float MapToRange(Range r)
    // public float PositionToAngle()
    public Vector3 OrientationToVector() {
        return AngleToVector(Orientation);
    }

    public static Vector3 AngleToVector(float angle) {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    // public Vector3 VectorHeading()
    // public float GetMiniminAngleTo(Vector3 rotation)
    // public void ResetOrientation()
    // public float PredictNearestApproachTime(Bodi other, float timeInit, float timeEnd)
    // public float PredictNearestApproachDistance3(Bodi other, float timeInit, float timeEnd)
}

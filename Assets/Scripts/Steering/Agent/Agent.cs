using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[AddComponentMenu("Steering/InteractiveObject/Agent")]
public class Agent : Bodi
{

    [Tooltip("Radio interior de la IA")]
    [SerializeField] protected float _interiorRadius = 1f;

    [Tooltip("Radio de llegada de la IA")]
    [SerializeField] protected float _arrivalRadius = 5f;

    [Tooltip("Ángulo interior de la IA")]
    [SerializeField] protected float _interiorAngle = 5.0f; // ángulo sexagesimal.

    [Tooltip("Ángulo exterior de la IA")]
    [SerializeField] protected float _exteriorAngle = 15.0f; // ángulo sexagesimal.


    // AÑADIR LAS PROPIEDADES PARA ESTOS ATRIBUTOS. SI LO VES NECESARIO.

    public float InteriorRadius {
        get => _interiorRadius;
        set => _interiorRadius = value;
    }

    public float ArrivalRadius {
        get => _arrivalRadius;
        set => _arrivalRadius = value;
    }

    public float InteriorAngle {
        get => _interiorAngle;
        set => _interiorAngle = value;
    }

    public float ExteriorAngle {
        get => _exteriorAngle;
        set => _exteriorAngle = value;
    }

    protected virtual void Awake() {
    }

    public void CopyFrom(Agent source) {
        _mass = source._mass;
        _maxSpeed = source._maxSpeed;
        _maxAcceleration = source._maxAcceleration;
        _maxAngularAcc = source._maxAngularAcc;

        _acceleration = source._acceleration;
        _angularAcc = source._angularAcc;
        _velocity = source._velocity;
        _rotation = source._rotation;
        _orientation = source._orientation;

        _interiorRadius = source.InteriorRadius;
        _arrivalRadius = source.ArrivalRadius;
        _interiorAngle = source.InteriorAngle;
        _exteriorAngle = source.ExteriorAngle;

        Position = source.Position;
    }

    // AÑADIR LO NECESARIO PARA MOSTRAR LA DEPURACIÓN. Te puede interesar los siguientes enlaces.
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html
    // https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html
    // https://docs.unity3d.com/ScriptReference/Gizmos.DrawWireSphere.html
    // https://docs.unity3d.com/ScriptReference/Gizmos-color.html
}

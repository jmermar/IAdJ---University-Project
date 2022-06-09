using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum Bando {
    Rojo, Azul
}

public abstract class AgentNPC : Agent
{
    [Header("Visual")]
    [SerializeField] private GameObject selectedIndicator;

    private Actions accion;

    // Combate
    [Header("Combate")]
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] private Bando bando;

    private AgentNPC objetivoAtaque;
    private AgentNPC[] enemigos;

    public AgentNPC ObjetivoAtaque { get => objetivoAtaque; set => objetivoAtaque = value; }
    public AgentNPC[] Enemigos {get => enemigos; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public Bando Bando { get => bando; }

    // Movimiento
    [Header("Movement")]
    [SerializeField] private Arbitrator arbitrator;
    [SerializeField] protected float _speedBase = 1f;
    [SerializeField] private Map map;

    private Steering steer;
    protected TerrainType good;
    protected TerrainType bad;
    private Formation formation;
    private bool reachedTarget;
    public bool ReachedTarget { get => reachedTarget; }

    public float SpeedBase {
        get => _speedBase;
        set => _speedBase = Mathf.Max(0, value);
    }

    public Map Map { get => map; set => map = value; }
    public Arbitrator Arbitrator { get => arbitrator; set => arbitrator = value; }
    public Formation Formation {get => formation;}
    protected Actions Accion { get => accion;}


    protected override void Awake()
    {
        base.Awake();
        Map = GameObject.FindObjectOfType<Map>();
        this.steer = new Steering();
        Speed = 0;
        AngularAcc = 0;
        enemigos = GameObject.FindObjectsOfType<AgentNPC>().Where(a => a != this && a.bando != bando).ToArray();
    }


    // Movimiento

    public bool MoveToTarget(Vector3 target) {
        if (arbitrator.GotoTarget(target)) {
            if (formation != null) formation.RemoveCharacter(this);
            reachedTarget = false;
            return true;
        }
        return false;
    }

    public void MoveToTargetLRTA(Vector3 target) {
        if (formation != null) formation.RemoveCharacter(this);
        arbitrator.GotoLRTA(target);
        reachedTarget = false;
    }

    public void StopMoving() {
        arbitrator.CancelMovement();
    }

    public bool IsMovingToTarget() {
        return arbitrator.IsMovingToTarget();
    }

    // Combate
    public abstract void RealizarAtaque(AgentNPC a);

    public bool InRangeToAttack(AgentNPC a) {
        return (a.Position - Position).magnitude <= AttackRange;
    }

    public bool Atacar(AgentNPC a) {
        if (Velocity.magnitude <= 0.001f) {
            RealizarAtaque(a);
            return true;
        }

        return false;
    }

    // Formaciones

    public void OnEnterFormation(Formation f) {
        if (formation != null) {
            formation.RemoveCharacter(this);
        }

        formation = f;

        arbitrator.OnFormationEnter(f);
    }

    public void OnExitFormation(Formation f) {
        formation = null;
        arbitrator.OnFormationExit(f);
    }

    public bool MoveLeaderToTarget(Vector3 target) {
        if (formation != null && formation.IsLeader(this)) {
            return arbitrator.GotoTarget(target);
        }
        return false;
    }

    public void MoveToNewSlot(Vector3 target) {
        if (formation == null) return;
        arbitrator.GotoTarget(target);
    }

    void UpdateFormation() {
    }

    // Update is called once per frame
    public virtual void Update()
    {
        UpdateTerrainParameters();

        ApplySteering();

        if (formation != null) UpdateFormation();
    }


    public void ReachedGoal() {
        if (formation != null && formation.Estado == Formation.Status.OffsetMove) {
            formation.ReachedGoal(this);
        }

        reachedTarget = true;
    }

    // Actualiza la velocidad máxima según sea el terreno que pisamos
    private void UpdateTerrainParameters() {
        Vector3 origin = transform.position + Vector3.up * 10;
        Vector3 direction = -Vector3.up;

        MaxSpeed = SpeedBase;

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(origin,
            direction,
            out hitInfo,
            Mathf.Infinity,
            1<<11);
        if (hit) {
            Terrain t = hitInfo.collider.GetComponent<Terrain>();
            if (t.Tipo == good) {
                MaxSpeed *= 2;
            } else if (t.Tipo == bad) {
                MaxSpeed *=  0.5f;
            }
        }

        // Si no estamos pisando ningún terreno dejamos la velocidad máxima a la base
        // sin embargo en un principio este caso nunca se va a dar (a no ser que se produzca un fallo)
    }

    private void ApplySteering()
    {
        if (steer.linear.magnitude < 0.01f) {
            Velocity *= 0.9f;
            Acceleration *= 0f;
        } else {
            Acceleration = steer.linear;
            if (Acceleration.magnitude > 0.001)
                Velocity += Time.deltaTime * Acceleration;
            if (Velocity.magnitude > 0.001)
                Position += Time.deltaTime * Velocity;
        }

        if (Mathf.Abs(steer.angular) < 0.01f) {
            Rotation *= 0.9f;
            AngularAcc = 0;
        } else {
            AngularAcc = steer.angular;
            if (Mathf.Abs(AngularAcc) > 0.01)
                Rotation += Time.deltaTime * AngularAcc;

            if (Mathf.Abs(Rotation) > 0.01)
                Orientation += Time.deltaTime * Rotation;
        }

        // Aplicar las actualizaciones a la componente Transform
        transform.rotation = new Quaternion(); //Quaternion.identity;
        transform.Rotate(Vector3.up, Orientation);
    }

    public virtual void LateUpdate()
    {
        // Reseteamos el steering final.
        this.steer = new Steering();

        if (Arbitrator != null) {
            this.steer = Arbitrator.GetSteering(this);
        }

        if (Mathf.Abs(steer.angular) > MaxAngularAcc)
        {
            steer.angular = Mathf.Sign(steer.angular) * MaxAngularAcc;
        }

        if (steer.linear.magnitude > MaxAcceleration) {
            steer.linear = steer.linear.normalized * MaxAcceleration;
        }
    }



    // Si el jugador intenta seleccionar al agente
    public bool TrySelect() {
        selectedIndicator.SetActive(true);

        return true;
    }

    public void Unselect() {
        selectedIndicator.SetActive(false);
    }
}

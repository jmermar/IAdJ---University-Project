using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : Seek
{
    [SerializeField] private Vector3[] _points;
    [SerializeField] private int _currentPoint = 0;

    [SerializeField] private float _radius = 2f;

    [SerializeField] private int _direction = 1;

    private bool onGoal = false;

    public Vector3[] Points { get => _points; set => _points = value; }
    public int CurrentPoint { get => _currentPoint; set => _currentPoint = value; }
    public float Radius { get => _radius; set => _radius = value; }
    public int Direction { get => _direction; set => _direction = value; }
    public bool OnGoal { get => onGoal; set => onGoal = value; }

    void Start()
    {
    }

    void OnDrawGizmos() {
        if (Global.Debug) {
            if (Points == null) return;
            Gizmos.color = Color.red;
            foreach(Vector3 point in Points) {
                Gizmos.DrawSphere(point, 0.25f);
            }
        }
    }

    protected override void Awake() {
        base.Awake();
        Target = TargetAgent.CreateTarget();
    }

    public override Steering GetSteering(Agent agent)
    {
        if(Points == null || Points.Length == 0) return new Steering();
        Target.Position = Points[CurrentPoint];
        OnGoal = false;

        if ((Target.Position - agent.Position).magnitude <= Radius) {
            CurrentPoint += Direction;
            if(CurrentPoint < 0 || CurrentPoint >= Points.Length) {
                CurrentPoint -= Direction;
                OnGoal = true;
                return null;
            }
        }

        return base.GetSteering(agent);
    }
}

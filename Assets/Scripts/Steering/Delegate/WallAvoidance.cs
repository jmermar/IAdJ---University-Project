using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidance : Seek
{
    [SerializeField] private int _rays = 3;
    [SerializeField] private float _avoidDistance = 2f;
    [SerializeField] private float _lookAhead = 1.0f;
    [SerializeField] private float _spread = 30f;
    [SerializeField] private int _layerMask = 0;

    public float AvoidDistance { get => _avoidDistance; set => _avoidDistance = value; }
    public float LookAhead { get => _lookAhead; set => _lookAhead = value; }
    public int LayerMask { get => _layerMask; set => _layerMask = value; }
    public int Rays { get => _rays; set => _rays = value; }
    public float Spread { get => _spread; set => _spread = value; }

    protected class Hit {
        public Vector3 TargetPosition;
        public float Distance;
    }
    void Start()
    {
    }

    protected Hit CastRay(float distance, float angle, Agent agent) {
        Vector3 origin = agent.Position;
        origin.y = 0.6f;

        Vector3 direction = Quaternion.AngleAxis(-angle, Vector3.up) * agent.Velocity.normalized;

        bool collision = Physics.Raycast(origin, direction, out RaycastHit info, distance, LayerMask);
        if (collision) {
            Hit ret = new Hit();

            Vector3 hitPoint = info.point;
            hitPoint.y = 0;

            Vector3 normal = info.normal;
            normal.y = 0;
            normal = normal.normalized;

            ret.Distance = distance;
            ret.TargetPosition = hitPoint +  normal * AvoidDistance;
            
            return ret;
        } else {
            return null;
        }
    }

    protected override void Awake() {
        base.Awake();
        Target = TargetAgent.CreateTarget();
    }

    protected Hit GetShortest(Hit h1, Hit h2) {
        if (h1 == null) return h2;
        if (h2 == null) return h1;
        return (h1.Distance < h2.Distance) ? h1 : h2;
    }

    public override Steering GetSteering(Agent agent)
    {
        Hit shortest = null;

        float step = Spread * 2f / (Rays);
        for(int i = 0; i < Rays; i++) {
            float delta = -Spread + i * step;
            shortest = GetShortest(shortest, CastRay(LookAhead, delta, agent));
        }
    
        if (shortest == null) return null;    
        Target.Position = shortest.TargetPosition;
        return base.GetSteering(agent);
    }

}

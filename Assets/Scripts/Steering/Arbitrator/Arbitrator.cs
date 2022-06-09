using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AStarSteering))]
[RequireComponent(typeof(LRTASteering))]
public abstract class Arbitrator : MonoBehaviour
{
    [SerializeField]private string _name;
    private AStarSteering astar;
    private LRTASteering lrta;
    private FormationSteering fs;
    private Flee flee;

    private float fleeSavedWeight = 0;

    protected List<SteeringBehavior> listSteerings;

    public string Name { get => _name; set => _name = value; }


    public virtual void Awake() {
        astar = GetComponent<AStarSteering>();
        lrta = GetComponent<LRTASteering>();
        fs = GetComponent<FormationSteering>();
        listSteerings = new List<SteeringBehavior>();
        foreach(SteeringBehavior sb in GetComponents<SteeringBehavior>()) {
            if (sb.Standalone) listSteerings.Add(sb);
        }
    }

    public abstract Steering GetSteering(AgentNPC a);

    public void GotoLRTA(Vector3 target) {
        astar.CancelMovement();
        lrta.MoveTo(target);
    }

    public bool IsMovingToTarget() {
        return astar.IsActive() || lrta.IsActive();
    }

    public void CancelMovement() {
        astar.CancelMovement();
        lrta.CancelMovement();
    }

    public bool GotoTarget(Vector3 target) {
        if(astar.MoveTo(target)) {
            lrta.CancelMovement();
            return true;
        }
        return false;
    }

    public void OnFormationEnter(Formation f) {
        fs.EnterFormation(f);
        astar.CancelMovement();
        lrta.CancelMovement();
    }

    public void OnFormationExit(Formation f) {
        fs.ExitFormation(f);
    }

}

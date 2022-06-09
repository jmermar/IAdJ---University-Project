using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AgentNPC))]
public class SteeringBehavior : MonoBehaviour
{
    public enum GroupSteering {
        Formation,
        Collision,
        Move,
        Flee
    }
    [SerializeField] private Agent target;
    [SerializeField] private GroupSteering group;
    [SerializeField] private float weight = 1;
    [SerializeField] private bool standalone = true;
    private AgentNPC agent;

    public AgentNPC Agent {get => agent; }

    public Agent Target { get => target; set => target = value; }
    public GroupSteering Group { get => group; set => group = value; }
    public float Weight { get => weight; set => weight = value; }
    public bool Standalone { get => standalone; set => standalone = value; }

    public virtual Steering GetSteering(Agent agent)
    {
        return null;
    }


    protected virtual void Awake() {
        agent = GetComponent<AgentNPC>();
    }

    protected virtual void OnGUI()
    {
        //Para la depuración te puede interesar que se muestre el nombre
        // del steeringbehaviour sobre el personaje.
        // Te puede ser util Rect() y GUI.TextField()
        // https://docs.unity3d.com/ScriptReference/GUI.TextField.html
    }
}

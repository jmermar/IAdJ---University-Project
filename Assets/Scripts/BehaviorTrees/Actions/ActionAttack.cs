using UnityEngine;
 
using Pada1.BBCore;           
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/Atacar")]
public class ShootOnce : BBUnity.Actions.GOAction
{
    [InParam("target")]
    public AgentNPC target;

    private AgentNPC agent;

    public override void OnStart() {
        agent = gameObject.GetComponent<AgentNPC>();
        agent.StopMoving();
    }

    public override TaskStatus OnUpdate()
    {
        if (agent.Atacar(target)) {
            return TaskStatus.COMPLETED;
        }

        return TaskStatus.RUNNING;
    }

}
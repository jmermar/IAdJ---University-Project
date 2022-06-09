using UnityEngine;
 
using Pada1.BBCore;           
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/MoverAEnemigo")]
public class  ActionMoveToEnemy : BBUnity.Actions.GOAction
{
    private AgentNPC target;
    private AgentNPC agent;

    public override void OnStart() {
        agent = gameObject.GetComponent<AgentNPC>();
        target = agent.ObjetivoAtaque;
        agent.MoveToTarget(target.Position);
    }

    public override TaskStatus OnUpdate()
    {
        if (agent.InRangeToAttack(target)) {
            return TaskStatus.COMPLETED;
        }

        if (agent.ReachedTarget) return TaskStatus.FAILED;

        return TaskStatus.RUNNING;
    }

}

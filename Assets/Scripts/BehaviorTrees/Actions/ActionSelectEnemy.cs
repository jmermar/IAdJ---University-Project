using UnityEngine;
 
using Pada1.BBCore;           
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/SeleccionarEnemigo")]
public class  ActionSelectEnemy : BBUnity.Actions.GOAction
{

    [InParam("maxDistance")]
    float maxDistance = 6f;

    public override TaskStatus OnUpdate()
    {
        var agent = gameObject.transform.GetComponent<AgentNPC>();

        // Elije el enemigo más cercano
        AgentNPC enemy = agent.Enemigos[0];
        foreach(var e in agent.Enemigos) {
            if ((e.Position - agent.Position).magnitude < (enemy.Position - agent.Position).magnitude) {
                enemy = e;
            }
        }

        if ((enemy.Position-agent.Position).magnitude > maxDistance) return TaskStatus.FAILED;


        agent.ObjetivoAtaque = enemy;

        return TaskStatus.COMPLETED;
    }

}
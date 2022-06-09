using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendedArbitrator : Arbitrator
{
    public override Steering GetSteering(AgentNPC a)
    {
        Steering res = new Steering();
        foreach(SteeringBehavior sb in listSteerings) {
            if (sb.Weight >= 0.001f) {
                Steering s = sb.GetSteering(a);
                if (s == null) s = new Steering();
                res.linear += s.linear * sb.Weight;
                res.angular += s.angular * sb.Weight;
            }
        }
        return res;
    }
}

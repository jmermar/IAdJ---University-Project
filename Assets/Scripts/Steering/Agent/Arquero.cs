using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arquero : AgentNPC
{
    public override void RealizarAtaque(AgentNPC a)
    {
        
    }

    void Start() {
        Arbitrator = GetComponent<Arbitrator>();

        good = TerrainType.Bosque;
        bad = TerrainType.Arena;

        InteriorRadius = 0.125f;
        ArrivalRadius = 2f;

        SpeedBase = 4.5f;
        MaxAcceleration = 100;
        MaxAngularAcc = 1000;
        MaxRotation = 10000;

        AttackRange = 8;
    }
}

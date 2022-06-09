using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centinela : AgentNPC
{
    public override void RealizarAtaque(AgentNPC a)
    {
        
    }

    void Start() {
        Arbitrator = GetComponent<Arbitrator>();

        good = TerrainType.Camino;
        bad = TerrainType.Bosque;

        InteriorRadius = 0.125f;
        ArrivalRadius = 2f;

        SpeedBase = 3;
        MaxAcceleration = 20;
        MaxAngularAcc = 1000;
        MaxRotation = 1000;

        AttackRange = 2;
    }
}

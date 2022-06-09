using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAgent : Agent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static TargetAgent CreateTarget() {
        GameObject entity = new GameObject();
        return entity.AddComponent<TargetAgent>();
    }
}

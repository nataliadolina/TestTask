using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClearPath : EventBase
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    public override void Execute()
    {
        agent.ResetPath();
        agent.isStopped = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PersecutionStateBase : State
{
    [SerializeField] protected float speed;

    protected Transform destination;
    protected NavMeshAgent agent;

    protected override void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        agent.speed = speed;

        SetDestination();
    }

    protected abstract void SetDestination();

    public override void Run()
    {
        agent.isStopped = false;
        agent.SetDestination(destination.position);
    }
}

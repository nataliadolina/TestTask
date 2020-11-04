using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PersecutionStateBase : State
{
    [SerializeField] protected float speed;

    protected StatesExecuter stateExecuter;
    protected Transform destination;
    protected NavMeshAgent agent;

    protected override void Start()
    {
        stateExecuter = GetComponentInParent<StatesExecuter>();

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

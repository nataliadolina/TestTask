using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersecutionState : State
{
    [SerializeField] private float speed;

    private Transform playerTransform;
    private NavMeshAgent agent;

    protected override void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        agent.speed = speed;

        playerTransform = FindObjectOfType<Player>().transform;
    }

    public override void Run()
    {
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);
        
        Debug.Log(playerTransform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public abstract class AttackState : State
{
    [SerializeField] private int harm;
    [SerializeField] private float waitTime;
    [SerializeField] private State nextState;

    protected UnitDataa unitData;
    protected StatesExecuter statesExecuter;

    private float currentTime;
    private Animator animator;

    protected NavMeshAgent agent;

    protected virtual void SetAim()
    {
        
    }

    protected override void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();

        currentTime = 0.0f;
        statesExecuter = GetComponentInParent<StatesExecuter>();
        animator = GetComponentInParent<Animator>();
        unitData = GetComponentInParent<UnitDataa>();

        SetAim();
    }

    public override void Run()
    {
        agent.isStopped = true;
        agent.ResetPath();

        if (unitData.aim != null)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= waitTime)
            {
                animator.SetTrigger("Atack");
                unitData.aim.HitPoint = harm;

                if (unitData.aim.HitPoint <= 0)
                {

                    statesExecuter.CurrentState = nextState;
                    return;
                }
                currentTime = 0.0f;
            }
        }
        
        else
        {
            statesExecuter.CurrentState = nextState;
        }

    }
}

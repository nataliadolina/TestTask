using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitSystem;

public class AttackState : State
{
    [SerializeField] private int harm;
    [SerializeField] private float waitTime;

    private float currentTime;
    private Animator animator;

    protected override void Start()
    {
        currentTime = 0.0f;
        animator = GetComponentInParent<Animator>();
    }

    public override void Run()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= waitTime)
        {
            Debug.Log("attack");
            animator.SetTrigger("Atack");
            currentTime = 0.0f;
        }
        
    }
}

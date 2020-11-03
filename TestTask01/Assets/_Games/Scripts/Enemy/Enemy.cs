using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private StateBase startState;
    [SerializeField] private int hitPoint;
    public int HitPoint
    {
        get => hitPoint;
        set
        {
            hitPoint -= value;
        }
    }

    public Animator animator;

    private StateBase[] states;
    
    private void Awake()
    {
        states = GetComponentsInChildren<StateBase>();
        animator = GetComponent<Animator>();
    }

    public void SwitchToTheNextState(IState nextState)
    {
        foreach (var state in states)
            state.StopAllCoroutines();
        StartCoroutine(nextState.Action());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class StateHandler : MonoBehaviour
{
    [SerializeField] private float detectionRadius;
    [SerializeField] private float attackRadius;

    [SerializeField] private StateBase idleState;
    [SerializeField] private StateBase persecutionState;
    [SerializeField] private StateBase attackState;
    [SerializeField] private StateBase deadState;

    private enum States
    {
        IDLE,
        PERSECUTION,
        ATTACK,
        DEAD
    }

    private States currentState;

    private StateBase[] states;
    private Transform aim;

    private void Start()
    {
        aim = CharacterControl.Instance.playerTransform;
        states = GetComponentsInChildren<StateBase>();
        currentState = States.IDLE;
        StartCoroutine(idleState.Action());
    }
    private void Update()
    {
        float distanceToAim = CountMagnitude();
        if (distanceToAim < detectionRadius & distanceToAim > attackRadius & currentState != States.PERSECUTION)
        {
            SwitchToState(persecutionState);
            currentState = States.PERSECUTION;
        }
        else if (distanceToAim <= attackRadius & currentState != States.ATTACK)
        {
            SwitchToState(attackState);
            currentState = States.ATTACK;
        }
        else if (currentState != States.IDLE)
        {
            currentState = States.IDLE;
            SwitchToState(idleState);
        }
    }

    private float CountMagnitude()
    {
        return Vector3.SqrMagnitude(aim.position - transform.position);
    }

    public void SwitchToState(IState nextState)
    {
        foreach (var state in states)
            state.StopAllCoroutines();
        StartCoroutine(nextState.Action());
    }

}

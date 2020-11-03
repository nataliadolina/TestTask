using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesExecuter : MonoBehaviour
{
    [SerializeField] private State idleState;
    [SerializeField] private State persecutionState;
    [SerializeField] private State attackingState;

    [SerializeField] private State currentState;
    public State CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    void Update()
    {
        currentState.Run();
    }
}

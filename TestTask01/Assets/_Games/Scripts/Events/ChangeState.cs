using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeState : EventBase
{
    [SerializeField] private State toState;

    private StatesExecuter statesExecuter;
    
    private void Start()
    {
        statesExecuter = GetComponentInParent<StatesExecuter>();
    }

    public override void Execute()
    {
        statesExecuter.CurrentState = toState;
    }
}

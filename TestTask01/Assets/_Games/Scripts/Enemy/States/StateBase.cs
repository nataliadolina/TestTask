using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase : MonoBehaviour, IState
{
    [SerializeField] protected StateBase nextState;
    protected Enemy enemy;

    public abstract IEnumerator Action();

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }
}

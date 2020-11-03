using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase : MonoBehaviour, IState
{
    protected Enemy enemy;

    public abstract IEnumerator Action();

    protected virtual void Init()
    {

    }
    protected void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        Init();
    }

    
}

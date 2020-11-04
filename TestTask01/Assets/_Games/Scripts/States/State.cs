using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State : MonoBehaviour
{
    protected bool isFinished = false;

    protected virtual void Start()
    {

    }
    public abstract void Run();
}

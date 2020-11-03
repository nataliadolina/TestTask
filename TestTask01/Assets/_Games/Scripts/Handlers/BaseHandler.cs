using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseHandler : MonoBehaviour
{
    [SerializeField] private TypeTarget typeTarget;

    private EventBase[] events;
    protected Predicate<Transform> typeChecking;

    private void Start()
    {
        events = GetComponentsInChildren<EventBase>();
        typeChecking = TypeTargetHandler.GetFunc(typeTarget);
    }

    protected void ExecuteAllEvents()
    {
        foreach (var _event in events)
        {
            _event.Execute();
        }
    }
}

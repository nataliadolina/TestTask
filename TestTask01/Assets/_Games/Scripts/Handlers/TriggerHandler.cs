using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : BaseHandler
{
    private void OnTriggerEnter(Collider other)
    {
        if (typeChecking.Invoke(other.transform))
        {
            ExecuteAllEvents();
        }
    }
}

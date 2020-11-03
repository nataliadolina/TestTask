using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExitHandler : BaseHandler
{
    private void OnTriggerExit(Collider other)
    {
        if (typeChecking.Invoke(other.transform))
        {
            ExecuteAllEvents();
        }
    }
}

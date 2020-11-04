using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursuingState : PersecutionStateBase
{
    protected override void SetDestination()
    {
        destination = FindObjectOfType<Player>().transform;
    }
}

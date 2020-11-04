using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : AttackState
{
    protected override void SetAim()
    {
        unitData.aim = FindObjectOfType<PlayerPersecutionState>().UnitData;
    }
}

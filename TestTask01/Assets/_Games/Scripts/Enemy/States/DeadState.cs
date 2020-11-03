using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : StateBase
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    public override IEnumerator Action()
    {
        Destroy(enemy.gameObject);
        yield return null;
    }
}

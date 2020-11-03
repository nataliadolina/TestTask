using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : StateBase
{
    [SerializeField] int attackHarm = 2;
    [SerializeField] Vector2 coolDownAttackTime;   

    private int aimHelth;

    protected override void Init()
    {
        aimHelth = CharacterControl.Instance.HitPoint;
    }

    public override IEnumerator Action()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(coolDownAttackTime.x, coolDownAttackTime.y));
            enemy.animator.SetTrigger("Attack");
            aimHelth = attackHarm;
        }
    }
}

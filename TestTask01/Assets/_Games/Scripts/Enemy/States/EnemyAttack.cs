using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttack : StateBase
{
    [SerializeField] int attackHarm = 2;
    [SerializeField] Vector2 coolDownAttackTime;

    [SerializeField] public static float attackRadius = 2.0f;
    [SerializeField] private StateBase previousState;
    

    private int aimHelth;
    private Transform aim;

    private void Start()
    {
        aim = CharacterControl.Instance.playerTransform;

        aimHelth = CharacterControl.Instance.HitPoint;
    }

    public override IEnumerator Action()
    {
        float distToAim = Vector3.SqrMagnitude(aim.position - transform.position);
        if (distToAim > Mathf.Pow(attackRadius, 2))
        {
            enemy.SwitchToTheNextState(previousState);
        }
        else
        {
            while (enemy.HitPoint > 0)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(coolDownAttackTime.x, coolDownAttackTime.y));
                aimHelth = attackHarm;
                enemy.animator.SetTrigger("Attack");
            }
            enemy.SwitchToTheNextState(nextState);
        }
    }
}

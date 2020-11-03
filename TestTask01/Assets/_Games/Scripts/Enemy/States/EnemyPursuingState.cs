using System.Collections;
using Character;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPursuingState : StateBase
{
    [SerializeField] private float speed = 6.0f;
    [SerializeField] public static float detectionRadius = 10.0f;

    [SerializeField] StateBase previousState;

    public enum Aim
    {
        Player,
        Enemy
    }

    private Transform aim;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        agent.speed = speed;
    }

    public override IEnumerator Action()
    {
        Vector3 aimPos = CharacterControl.Instance.playerTransform.position;
        float distToAim = Vector3.SqrMagnitude(aimPos - transform.position);
        if (distToAim <= detectionRadius * detectionRadius)
        {
            if (distToAim > EnemyAttack.attackRadius * EnemyAttack.attackRadius)
            {
                Debug.Log(agent);
                agent.SetDestination(aimPos);
                yield return new WaitForFixedUpdate();
            }
            else
            {
                agent.ResetPath();
                enemy.SwitchToTheNextState(nextState);
            }
        }
        else
        {
            agent.ResetPath();
            enemy.SwitchToTheNextState(previousState);
        }
    }
}

using System.Collections;
using Character;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPursuingState : StateBase
{
    [SerializeField] private float speed = 6.0f;

    private NavMeshAgent agent;

    private Transform aim;
    
    protected override void Init()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        Debug.Log(agent);
        agent.speed = speed;
    }

    public override IEnumerator Action()
    {
        Debug.Log(agent);
        Vector3 aimPos = CharacterControl.Instance.playerTransform.position;
        float distToAim = Vector3.SqrMagnitude(aimPos - transform.position);
        while (true)
        {
            agent.SetDestination(aimPos);
            yield return new WaitForFixedUpdate();
        }

    }
}

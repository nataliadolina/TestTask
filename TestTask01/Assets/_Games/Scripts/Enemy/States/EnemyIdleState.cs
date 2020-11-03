using System.Collections;
using UnityEngine;
using Character;

public class EnemyIdleState : StateBase
{
    private Transform aim;

    private void Start()
    {
        aim = CharacterControl.Instance.playerTransform;
        StartCoroutine(Action());
    }
    
    public override IEnumerator Action()
    {
        float distToPlayer = Vector3.SqrMagnitude(aim.position - transform.position);
        while (distToPlayer > Mathf.Pow(EnemyPursuingState.detectionRadius, 2))
        {
            yield return new WaitForFixedUpdate();
        }
        enemy.SwitchToTheNextState(nextState);
    }
    
}

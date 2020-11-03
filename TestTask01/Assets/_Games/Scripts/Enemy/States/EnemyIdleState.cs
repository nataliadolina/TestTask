using System.Collections;
using UnityEngine;
using Character;

public class EnemyIdleState : StateBase
{
    private void Start()
    { 
        
    }
    
    public override IEnumerator Action()
    {
        Transform aim = CharacterControl.Instance.playerTransform;
        float distToPlayer = Vector3.SqrMagnitude(aim.position - transform.position);
        while (true)
        {
            yield return new WaitForFixedUpdate();
        }
    }
    
}

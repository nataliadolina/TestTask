using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPersecutionState : PersecutionStateBase
{
    [SerializeField] private State previousState;

    private StatesExecuter stateExecuter;
    private List<Transform> enemies;

    private void Awake()
    {
        enemies = new List<Transform>();
        foreach (var enemy in FindObjectsOfType<EnemyPursuingState>())
        {
            enemies.Add(enemy.transform);
        }
    }

    protected override void SetDestination()
    {
        destination = FindClosestEnemy();
    }

    public override void Run()
    {
        destination = FindClosestEnemy();
        if (destination != null)
        {
            agent.isStopped = false;
            agent.SetDestination(destination.position);
        }

        else
        {
            stateExecuter.CurrentState = previousState;
        }
    }

    private Transform FindClosestEnemy()
    {
        float minDistance = 400f;
        Transform transform = null;
        foreach (var enemyTransform in enemies)
        {
            if (enemyTransform == null)
            {
                continue;
            }

            float distance = Vector3.Magnitude(enemyTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                transform = enemyTransform;
            }
        }
        return transform;
    }
}

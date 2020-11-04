using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPersecutionState : PersecutionStateBase
{
    [SerializeField] private State previousState;

    private UnitDataa unitData;

    public UnitDataa UnitData
    {
        get => unitData;
    }


    public static List<UnitDataa> enemies;

    private void Awake()
    {
        unitData = GetComponentInParent<UnitDataa>();
        enemies = new List<UnitDataa>();
        foreach (var enemy in FindObjectsOfType<EnemyPursuingState>())
        {
            enemies.Add(enemy.GetComponent<UnitDataa>());
        }
    }

    protected override void SetDestination()
    {
        
    }

    public override void Run()
    {
        
        UnitDataa closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            destination = closestEnemy.transform;
            agent.isStopped = false;
            agent.SetDestination(destination.position);
            unitData.aim = closestEnemy;
        }

        else
        {
            stateExecuter.CurrentState = previousState;
            unitData.aim = null;
        }
    }

    private UnitDataa FindClosestEnemy()
    {
        UnitDataa[] enemies = FindObjectsOfType<UnitDataa>();

        float minDistance = 400f;
        UnitDataa closestEnemy = null;
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Player"))
            {
                continue;
            }
            Transform enemyTransform = enemy.transform;

            float distance = Vector3.Magnitude(enemyTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}

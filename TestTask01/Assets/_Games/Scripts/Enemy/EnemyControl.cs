using Character;
using UnitSystem;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

    public class EnemyControl : MonoBehaviour
    {
        [SerializeField] private float _speed = 6.0f;
        [SerializeField] private  float _detectionRadius = 10.0f;
        
        private enum State
        {
            IDLE,
            PURSUING,
            ATTACKING,
            DEAD
        }
        private State state;
        private Vector3 startingAnchor;
        
        private bool isDead;

        private float changePositionTimer;
        private float pursuitTimer = 0.0f;

        private NavMeshAgent agent;
        private Animator animator;
        private UnitData characterData;
        private EnemySpawner spawner;
        private GameObject enemyPanel;
        private UIManager uiManager;


        void Start()
        {
            changePositionTimer = Random.Range(3, 7);
            spawner = GetComponentInParent<EnemySpawner>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            characterData = GetComponent<UnitData>();
            characterData.Init();
            
            uiManager = UIManager.Instance;

            agent.speed = _speed;

            startingAnchor = transform.position;
            characterData.hpBar = uiManager.EnemyPanel;
        }

        void Update()
        {
            if (isDead)
                return;
            
            if (characterData.Stats.CurrentHealth == 0)
            {
                isDead = true;
                spawner.CurrentEnemyCount--;
                agent.isStopped = true;
                
                gameObject.SetActive(false);
                
                Destroy(GetComponent<SphereCollider>());
                Destroy(agent);
                Destroy(gameObject, 3);
                
                return;
            }
        
            Vector3 playerPosition = CharacterControl.Instance.transform.position;
            UnitData playerData = CharacterControl.Instance.Data;
        
            switch (state)
            {
                // выделить в отдельные объекты
                case State.IDLE:
                {
                    changePositionTimer -= Time.deltaTime;
                    if (changePositionTimer <= 0)
                    {
                        changePositionTimer = Random.Range(3, 7);
                        agent.isStopped = false;
                        agent.SetDestination(spawner.NextPosition());
                    }
                    
                    if (Vector3.SqrMagnitude(playerPosition - transform.position) < _detectionRadius * _detectionRadius)
                    {
                        pursuitTimer = 2.0f;
                        state = State.PURSUING;
                        agent.isStopped = false;
                    }
                }
                    break;
                case State.PURSUING:
                {
                    float distToPlayer = Vector3.SqrMagnitude(playerPosition - transform.position);
                    if (distToPlayer <= _detectionRadius * _detectionRadius)
                    {
                        pursuitTimer = 2.0f;

                        if (characterData.CanAttackTarget(playerData))
                        {
                            agent.ResetPath();
                            agent.velocity = Vector3.zero;

                            characterData.AttackTriggered();
                            characterData.Attack(playerData);
                            state = State.ATTACKING;
                            agent.isStopped = true;
                            animator.SetTrigger("Attack");
                        }
                    }
                    else
                    {
                        if (pursuitTimer > 0.0f)
                        {
                            pursuitTimer -= Time.deltaTime;

                            if (pursuitTimer <= 0.0f)
                            {
                                agent.SetDestination(startingAnchor);
                                state = State.IDLE;
                                enemyPanel.SetActive(false);
                            }
                        }
                    }
                
                    if (pursuitTimer > 0)
                        agent.SetDestination(playerPosition);
                }
                    break;
                case State.ATTACKING:
                {
                    if (!characterData.CanAttackReach(playerData))
                    {
                        state = State.PURSUING;
                        agent.isStopped = false;

                    }
                    else
                    {
                        if (characterData.CanAttackTarget(playerData) && characterData.TargetIsLive(playerData))
                        {
                            agent.ResetPath();
                            characterData.Attack(playerData);
                            characterData.AttackTriggered();
                            animator.SetTrigger("Attack");
                        }
                    }
                }
                    break;
            }
        }
    }
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class CharacterControl : MonoBehaviour
    {
        public static CharacterControl Instance { get; protected set; }
        public UnitData Data => characterData;
        
        public enum State
        {
            DEFAULT,
            ATTACKING
        }
        [HideInInspector] public State CurrentState;

        [SerializeField] private float _speed = 10.0f;
        
        private NavMeshAgent agent;
        private CharacterData characterData;
        
        private UnitData currentTargetCharacterData; //Текущая цель игрока
        private UnitData lastTegetCharacterData; //Предыдущая цель игрока

        private Vector3 playerSpawn;
        
        private bool isDead;
        
        private float turnSmoothVelocity;
        private float turnSmoothTime = 0.01f;
        private float deathTimer;

        private List<UnitData> enemieDatas = new List<UnitData>(); //Массив всех врагов поблизости 
        private UIManager uiManager;
        private Animator animator;
        private Weapon weapon;
        
        private void Awake()
        {
            Instance = this;
            characterData = GetComponent<CharacterData>();
            AttackState._characterControl = this;
            characterData.Init();
        }
        
        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();

            agent.speed = _speed;
            playerSpawn = transform.position;
            uiManager = UIManager.Instance;
            
            CurrentState = State.DEFAULT;
        }

        private void Update()
        {
            if (isDead) 
            {
                deathTimer += Time.deltaTime;
                if (deathTimer > 3.0f)
                    GoToRespawn();

                return;
            }

            if (characterData.Stats.CurrentHealth == 0)
            {
                isDead = true;
                deathTimer = 0.0f;
            }
            
            if (currentTargetCharacterData && !characterData.TargetIsLive(currentTargetCharacterData)) //Если цель игрока мертва, то...
            {
                enemieDatas.Remove(currentTargetCharacterData);
                currentTargetCharacterData = null;
                uiManager.EnemyPanel.gameObject.SetActive(false);
            }
            
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            if (direction.magnitude >= 0.1f && CurrentState != State.ATTACKING)
                Move(direction);
            
            else if (Input.GetMouseButtonDown(0))
            {
                TryFoundEnemy();

                if (currentTargetCharacterData && characterData.CanAttackReach(currentTargetCharacterData))
                {
                    StopAgent();
                    
                    var targetDirection = (currentTargetCharacterData.transform.position - transform.position).normalized;
                    targetDirection.y = 0f;
                    transform.rotation = Quaternion.LookRotation(targetDirection);
                
                    animator.SetTrigger("Attack");
                    CurrentState = State.ATTACKING;
                    characterData.Attack(currentTargetCharacterData);
                    characterData.AttackTriggered();
                }
            }
            else
                StopAgent();
        }

        void GoToRespawn()
        {
            agent.Warp(playerSpawn);
            agent.isStopped = true;
            agent.ResetPath();
            
            isDead = false;
            currentTargetCharacterData = null;
            CurrentState = State.DEFAULT;
        
            characterData.Stats.ChangeHealth(characterData.Stats.stats.health);
        }
        
        private void Move(Vector3 direction)
        {
            Vector3 newPosition = transform.position + direction * (Time.deltaTime * _speed);
            NavMeshHit hit;
            
            bool isValid = NavMesh.SamplePosition(newPosition, out hit, .6f, NavMesh.AllAreas);
            
            if (isValid)
            {
                agent.ResetPath();
                transform.position = hit.position;
                Rotate(direction);
            }
        }
        
        private void Rotate(Vector3 direction)
        {
            transform.localRotation = Quaternion.RotateTowards(from: transform.rotation,
                    to: Quaternion.LookRotation(direction), maxDegreesDelta: Time.deltaTime * 720);
        }

        private void StopAgent()
        {
            agent.velocity = Vector3.zero;
        }
        
        private void TryFoundEnemy()
        {
            if (enemieDatas.Count == 0 ||
                currentTargetCharacterData && enemieDatas.Contains(currentTargetCharacterData))
            {
                if (currentTargetCharacterData)
                    SwitchTarget();
                return;
            }
            
            
            float minDistance = 10000;

            foreach (var enemieData in enemieDatas)
            {
                if (!enemieData) continue;
                
                float enemyDistance = Vector3.SqrMagnitude(enemieData.transform.position - transform.position);

                if (!(enemyDistance < minDistance)) continue;
                
                minDistance = enemyDistance;
                currentTargetCharacterData = enemieData;
                SwitchTarget();
            }
        }

        private void SwitchTarget()
        {
            uiManager.EnemyPanel.gameObject.SetActive(true);
            uiManager.EnemyPanel.fillAmount = (float) currentTargetCharacterData.Stats.CurrentHealth /
                                                        currentTargetCharacterData.Stats.stats.health;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            UnitData target = other.GetComponent<UnitData>();
            if (target)
            {
                enemieDatas.Add(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            UnitData target = other.GetComponent<UnitData>();
            if (target)
            {
                enemieDatas.Remove(target);
            }
        }
    }
}

using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Character
{
    public class CharacterControl : MonoBehaviour
    {
        public static CharacterControl Instance;
        public UnitData Data => characterData;
        public Transform playerTransform;
        
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
        private Animator animator;

        [SerializeField] private int hitPoint;
        public int HitPoint
        {
            get => hitPoint;
            set
            {
                hitPoint -= value;
                if (hitPoint < 0)
                {
                    GoToRespawn();
                }
            }
        }


        private void Awake()
        {
            Instance = this;
            playerTransform = GetComponent<Transform>();
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
                return;
            }
            
            
            float minDistance = 10000;

            Debug.Log(enemieDatas);
            foreach (var enemieData in enemieDatas)
            {
                if (!enemieData) continue;
                
                float enemyDistance = Vector3.SqrMagnitude(enemieData.transform.position - transform.position);

                if (!(enemyDistance < minDistance)) continue;
                
                minDistance = enemyDistance;
                currentTargetCharacterData = enemieData;
            }
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

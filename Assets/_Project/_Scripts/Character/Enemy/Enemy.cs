using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterAnimatorManager))]
public class Enemy : MonoBehaviour
{
    
    public static List<Enemy> ActiveEnemies = new List<Enemy>();

    public EnemySO EnemyData;
  
    AIPath _aiPath;
    FollowerEntity _followerEntity;
    AIDestinationSetter _aiDestinationSetter;
    
    
    protected CharacterAnimatorManager _animatorManager;
    protected Transform _playerTransform;
    protected Health _health;
    protected bool _returningToPool;
    protected Rigidbody _rigidbody;
    protected EnemyHealth _enemyHealth;
    [SerializeField] protected GameObject _statusEffectPrefab;
    
    bool _isAttackAnimationPlaying = false;
    bool _isDead = false;
    public bool IsPushed;
    public bool StopAnimator;
    
    float _timeSinceLastAttack = 0f;
    
    Vector3 _startingAttackPosition;
    
    public enum EnemyState
    {
        Walking,
        Attacking,
        TornadoPull
    }
    
    int currentPathIndex = 0;

    public EnemyState _currentState = EnemyState.Walking;

    protected void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        //_aiPath = GetComponent<AIPath>();
        _followerEntity = GetComponent<FollowerEntity>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        _health = GetComponent<Health>();
        _animatorManager = GetComponent<CharacterAnimatorManager>();
        if (_animatorManager == null)
        {
            _animatorManager = GetComponentInChildren<CharacterAnimatorManager>();
        }
        
        if(!ActiveEnemies.Contains(this))
        {
            ActiveEnemies.Add(this);
        }
    }
   
    protected void OnEnable()
    {
        _isDead = false;
        if (_statusEffectPrefab != null)
        {
            _statusEffectPrefab.SetActive(false);
        }
        _followerEntity = GetComponent<FollowerEntity>();
        _aiDestinationSetter.target = _playerTransform;
        _currentState = EnemyState.Walking;
        if(!ActiveEnemies.Contains(this))
        {
            ActiveEnemies.Add(this);
        }
    }

    protected void OnDisable()
    {
        _isDead = true;
        if(!ActiveEnemies.Contains(this))
        {
            ActiveEnemies.Remove(this);
        }
    }


    protected virtual void Start()
    {
        _playerTransform = GameManager.Instance.GetPlayerTransform();
        if (_playerTransform == null)
        {
            Debug.LogError("Player transform is null!");
        }
        //_rigidbody.isKinematic = false;

        //StartCoroutine(InitializePathfinding());
    }
    
    public void SetEnemyData(EnemySO enemyData)
    {
        EnemyData = enemyData;
    }
    
    public void SetPlayerTransform(Transform playerTransform)
    {
        this._playerTransform = playerTransform;
    }
    public void SetPlayerTransformFromPool(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _returningToPool = false; 
    }
    
    public AIDestinationSetter GetAIDestinationSetter()
    {
        return _aiDestinationSetter;
    }

    protected virtual void Update()
    {
        if (IsPushed)
        {
            return;
        }

        _timeSinceLastAttack += Time.deltaTime;

        if (!_health.IsDead())
        {
            float distanceToPlayer = CalculateDistanceToPlayer();

            switch (_currentState)
            {
                case EnemyState.Walking:
                    HandleWalkingState(distanceToPlayer);
                    break;
                case EnemyState.Attacking:
                    HandleAttackingState(distanceToPlayer);
                    break;
                case EnemyState.TornadoPull:
                    HandleTornadoPullState();
                    break;
            }
        }
    }
    
    public bool IsDead()
    {
        if (_health.IsDead())
        {
            _isDead = true;
            return true;
        }
        _isDead = false;
        return false;
        
    }

    void HandleWalkingState(float distanceToPlayer)
    {
        _followerEntity.enabled = true;
        _followerEntity.maxSpeed = 2f;
        _aiDestinationSetter.enabled = true;
        _aiDestinationSetter.target = _playerTransform;
        _isAttackAnimationPlaying = false;
        _animatorManager.PlayWalkingAnimation();
        if (EnemySpawner.Instance.GetFormation() == EnemySpawner.FormationType.Fibonacci)
        {
            HandleSpiralMovement();
        }
        if (EnemySpawner.Instance.GetFormation() == EnemySpawner.FormationType.Square)
        {
            float radius = 10f;
            float offset = 2f;
            
            var normal = (transform.position - _playerTransform.position).normalized;
            var tangent = Vector3.Cross(normal, _playerTransform.up);
            
            {
                // _followerEntity.SetDestination(_playerTransform.position + normal * radius + tangent * offset);
                // _aiDestinationSetter.target = _playerTransform;
            }
        }

        if (distanceToPlayer <= EnemyData.AttackRange)
        {
            _currentState = EnemyState.Attacking;
            _animatorManager.StopWalkingAnimation();
        }
    }
   
    void HandleAttackingState(float distanceToPlayer)
    {
        _animatorManager.StopWalkingAnimation();

        if (_timeSinceLastAttack >= EnemyData.AttackCooldown)
        {
            StartAttackAnimation();
        }
        else
        {
            StopAttackAnimation();
        }

        if (distanceToPlayer > EnemyData.AttackRange)
        {
            _isAttackAnimationPlaying = false;
            _currentState = EnemyState.Walking;
            StopAttackAnimation();
        }
        else
        {
            StartAttackAnimation();
        }
    }
    
    void HandleTornadoPullState()
    {
        GameObject tornado = GameObject.FindGameObjectWithTag("Tornado");

        _followerEntity.enabled = false;
        float radius = 1f;
        float offset = 1f;
        var normal = (transform.position - tornado.transform.position).normalized;
        var tangent = Vector3.Cross(normal, tornado.transform.up);
        
        Vector3 pullPosition = Vector3.Lerp(transform.position, tornado.transform.position + normal * radius + tangent * offset, 1.5f * Time.deltaTime);
        transform.position = pullPosition;
        
        
        _animatorManager.StopWalkingAnimation();
        _animatorManager.StopAttackAnimation();
    }
    
   
    
    IEnumerator InitializePathfinding()
    {
        // Wait until the end of the frame to ensure all Start methods are called.
        yield return new WaitForEndOfFrame();

        float radius = 5f;
        float offset = 2f;
            
        var normal = (transform.position - _playerTransform.position).normalized;
        var tangent = Vector3.Cross(normal, _playerTransform.up);
        // Now it's safe to start pathfinding.
        _followerEntity.SetDestination(_playerTransform.position + normal * radius + tangent * offset);
    }

    void StartAttackAnimation()
    {
        _isAttackAnimationPlaying = true;
        _animatorManager.PlayAttackAnimation();
        _timeSinceLastAttack = 0f;
    }

    void StopAttackAnimation()
    {
        _isAttackAnimationPlaying = false;
        _animatorManager.StopAttackAnimation();
    }

    /*void HandleReturnToPool()
    {
        _animatorManager.StopWalkingAnimation();
        _returningToPool = true;
        StartCoroutine(DelayedReturn(gameObject, 5f));
    }
    
    IEnumerator DelayedReturn(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        StopAllCoroutines();
        SetPlayerTransformFromPool(GameManager.Instance.GetPlayerTransform());
        ObjectPoolManager.ReturnEnemyObjectToPool(obj);
        _health.Respawn();
        _returningToPool = false;
    }*/

    
    void DealDamageToPlayer()
    {
        if (_playerTransform == null)
        {
            return;
        }
        
        _playerTransform.GetComponent<Health>().TakeDamage(EnemyData.AttackDamage);
    }
    
   
    void HandleNormalMovement()
    {
        if (_currentState == EnemyState.Attacking)
        {
            // If the enemy is attacking, stop movement
            return;
        }
        float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
        float playerSeparationRange = 5f;

        Vector3 targetPosition = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);
        Vector3 directionToPlayer = targetPosition - transform.position;
        Vector3 finalDirection;

        // Only apply the separation logic if the enemy is within a certain range of the player
        if (distanceToPlayer <= playerSeparationRange)
        {
            Vector3 separationForce = CalculateSeparationForce();
            // Add the separation force to the direction towards the player
            finalDirection = directionToPlayer.normalized + separationForce;
        }
        else
        {
            // Just head towards the player
            finalDirection = directionToPlayer.normalized;
        }

        // Move towards the final direction
        transform.position = Vector3.MoveTowards(transform.position, transform.position + finalDirection, EnemyData.MoveSpeed * Time.deltaTime);

        // Make sure the enemy faces the player
        directionToPlayer.y = 0;

        // Define rotation speed
        float rotationSpeed = 360f; // You can adjust this value to get the rotation speed you prefer

        // Desired rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Rotate the enemy
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void HandleSpiralMovement()
    {
        if (_currentState == EnemyState.Attacking)
        {
            // If the enemy is attacking, stop movement
            return;
        }

        Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;

        // Radial Movement - This makes the enemy move towards the player.
        Vector3 radialMove = directionToPlayer * EnemyData.MoveSpeed * 0.8f * Time.deltaTime;

        // Tangential Movement - This makes the enemy move perpendicular to the direction of the player.
        Vector3 tangentialMove = Vector3.Cross(directionToPlayer, Vector3.up).normalized * EnemyData.MoveSpeed * 0.3f * Time.deltaTime;

        // Scale the tangentialMove by a factor to control the spiral speed. 
        

        tangentialMove *= 1.5f;
        
        // Combine the two motions.
        Vector3 combinedMove = radialMove + tangentialMove;
        
        Vector3 desiredMove = CalculateSpiralSeparationForce();
        combinedMove = Vector3.Lerp(combinedMove, desiredMove, 0.05f); // Interpolate for smoother transition


        transform.position += combinedMove;

        // Rotate the enemy to face the direction of movement
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.Lerp(directionToPlayer, combinedMove, 0.1f));
        float rotationSpeed = 360; // Adjust this value as per your requirement
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    Vector3 CalculateSeparationForce()
    {
            float desiredSeparation = 1.5f; // The distance at which the enemy will start to separate from other enemies
            Vector3 sum = Vector3.zero;
    
    
            int count = 0;

            var enemies = ActiveEnemies;
            foreach (var other in enemies)
            {
                if (other == this) continue; // skip self

                Vector3 positionDifference = transform.position - other.transform.position;

                // Ignore the y-axis difference
                positionDifference.y = 0f;

                float d = positionDifference.magnitude;
                if (d > 0 && d < desiredSeparation)
                {
                    // calculate vector headed away from myself
                    positionDifference.Normalize();
                    positionDifference /= d; // weight by distance
                    sum += positionDifference;
                    count++;
                }
            }

            if (count > 0)
            {
                sum /= count;
                sum *= EnemyData.MoveSpeed;
            }

            return sum * 0.5f;
        
    }

    Vector3 CalculateSpiralSeparationForce()
    {
        float desiredSeparation = 1.5f;
        Vector3 sum = Vector3.zero;

        NativeArray<float3> enemyPositions = new NativeArray<float3>(ActiveEnemies.Count, Allocator.TempJob);
        NativeArray<float3> results = new NativeArray<float3>(ActiveEnemies.Count, Allocator.TempJob);

        for (int i = 0; i < ActiveEnemies.Count; i++)
        {
            var enemyTransform = ActiveEnemies[i].transform;
            enemyPositions[i] = enemyTransform.position;
        }

        SeparationForceJob job = new SeparationForceJob
        {
            enemyPositions = enemyPositions,
            myPosition = transform.position,
            desiredSeparation = desiredSeparation,
            moveSpeed = EnemyData.MoveSpeed,
            results = results
        };

        JobHandle handle = job.Schedule(ActiveEnemies.Count, 256);
        handle.Complete();

        for (int i = 0; i < results.Length; i++)
        {
            sum += new Vector3(results[i].x, results[i].y, results[i].z);
        }

        enemyPositions.Dispose();
        results.Dispose();

        return sum * 0.5f;
    }


    protected virtual float CalculateDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
        return distanceToPlayer;
    }
    
   


   
}

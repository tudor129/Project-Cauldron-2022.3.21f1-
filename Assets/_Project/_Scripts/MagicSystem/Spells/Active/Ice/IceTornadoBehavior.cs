using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IceTornadoBehavior : SpellBehavior
{
    Dictionary<Collider, Coroutine> _activeCoroutines = new Dictionary<Collider, Coroutine>();
    
    public float changeDirectionInterval = 2f; // Time in seconds between direction changes
    public float maxDirectionChange = 180.0f; // Max degrees the tornado can turn per interval
    float _timeSinceLastChange = 0.0f;
    bool _isCoroutineRunning;

    
    Vector3 _targetDirection;
    

    [FormerlySerializedAs("pullStrength")] [SerializeField] float _pullStrength = 1f;
    
    FollowerEntity _followerEntity;
    
    protected GameObject _currentTarget;

    protected override void Awake()
    {
        base.Awake();
        _followerEntity = GetComponent<FollowerEntity>();
    }
    protected override void Start()
    {
        base.Start();
        _targetDirection = _randomDirection;
    }
   

    // Update is called once per frame
    protected override void Update()
    {
        if (_hasMovement)
        {
            
            // float radius = 5f;
            // float offset = 2f;
            //
            // var normal = (transform.position - _player.transform.position).normalized;
            // var tangent = Vector3.Cross(normal, _player.transform.up);
            //
            // _followerEntity.SetDestination(_player.transform.position + normal * radius + tangent * offset);
            
            float radius = 5f;
            float offset = 2f;
            float engageDistance = 10f;
            // Check if current target is null or too far away
            if (_currentTarget == null || Vector3.Distance(transform.position, _currentTarget.transform.position) > engageDistance)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if (enemies.Length > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, enemies.Length);
                    _currentTarget = enemies[randomIndex]; // Set new random target
                }
                else
                {
                    _currentTarget = _player.gameObject; // Default back to player if no enemies
                }
            }

            // Calculate position relative to the current target
            var normal = (transform.position - _currentTarget.transform.position).normalized;
            var tangent = Vector3.Cross(normal, _currentTarget.transform.up);

            // Set destination to orbit around the current target
            _followerEntity.SetDestination(_currentTarget.transform.position + normal * radius + tangent * offset);
            //_followerEntity.SetDestination(_currentTarget.transform.position);
           
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_activeCoroutines.ContainsKey(other))
        {
            var enemy = other.GetComponent<Enemy>();
            if (!enemy.IsDead())
            {
                enemy._currentState = Enemy.EnemyState.TornadoPull;
            
                int damage = Mathf.RoundToInt(_currentStats.DamageOverTime);
                Coroutine coroutine = StartCoroutine(DoTRoutine(
                    damage,
                    _currentStats.DamageOverTimeInitialDelay,
                    _currentStats.DamageOverTimeInterval,
                    _currentStats, other));
                _activeCoroutines.Add(other, coroutine);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            StopCoroutine(_activeCoroutines[other]);
            _activeCoroutines.Remove(other);
            _isCoroutineRunning = false;
            enemy._currentState = Enemy.EnemyState.Walking;
        }
    }
    
    IEnumerator DoTRoutine(int damagePerTick, float initialDelay, float tickInterval, Spell.Stats spellInfo, Collider other)
    {
        _isCoroutineRunning = true;
        yield return new WaitForSeconds(initialDelay);
        
        while (other != null && other.gameObject.activeInHierarchy)
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damagePerTick, false, spellInfo);
            yield return new WaitForSeconds(tickInterval);
        }
        if (_activeCoroutines.ContainsKey(other))
        {
            _activeCoroutines.Remove(other);
        }
    }
  

}

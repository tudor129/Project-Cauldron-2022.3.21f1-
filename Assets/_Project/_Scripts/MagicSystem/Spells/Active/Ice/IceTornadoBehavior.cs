using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IceTornadoBehavior : SpellBehavior
{
    public static List<IceTornadoBehavior> ActiveTornadoes = new List<IceTornadoBehavior>();
    Dictionary<Collider, Coroutine> _activeCoroutines = new Dictionary<Collider, Coroutine>();
    
    float _timeSinceLastChange = 0.0f;
    bool _isCoroutineRunning;
    
    FollowerEntity _followerEntity;
    
    protected Enemy _currentTarget;

    protected override void Awake()
    {
        base.Awake();
        _followerEntity = GetComponent<FollowerEntity>();
        ActiveTornadoes.Clear();
        if(!ActiveTornadoes.Contains(this))
        {
            ActiveTornadoes.Add(this);
        }
    }
    protected override void Start()
    {
        base.Start();
        CoroutineManager.Instance.StartManagedCoroutine(SpawnDecals(0.35f));
    }

    protected void OnEnable()
    {
        if(!ActiveTornadoes.Contains(this))
        {
            ActiveTornadoes.Add(this);
        }
    }
    void OnDisable()
    {
        if(!ActiveTornadoes.Contains(this))
        {
            ActiveTornadoes.Remove(this);
        }
    }


    // Update is called once per frame
    protected override void Update()
    {
        if (_hasMovement)
        {
            float radius = 5f;
            float offset = 2f;
            float engageDistance = 20f;
            float closeEnoughDistance = 0.2f;
            // Check if current target is null or too far away
            if (_currentTarget == null || Vector3.Distance(transform.position, _currentTarget.transform.position) > engageDistance)
            {
                //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                List<Enemy> enemies = Enemy.ActiveEnemies;
                //Enemy.ActiveEnemies = enemies;
                if (enemies.Count > 0)
                {
                    int randomIndex = Random.Range(0, enemies.Count);
                    _currentTarget = enemies[randomIndex]; // Set new random target
                }
                else
                {
                    _currentTarget = null; // Default back to player if no enemies
                }
            }
            // Calculate position relative to the current target
            // var normal = (transform.position - _currentTarget.transform.position).normalized;
            // var tangent = Vector3.Cross(normal, _currentTarget.transform.up);

            // Set destination to orbit around the current target
            //_followerEntity.SetDestination(_currentTarget.transform.position + normal * radius + tangent * offset);
            
            // Set destination to move towards the current target
            _followerEntity.SetDestination(_currentTarget.transform.position);
           
            // Check if reached the target
            if (Vector3.Distance(transform.position, _currentTarget.transform.position) <= closeEnoughDistance)
            {
                _currentTarget = null; // Reset target, so it will find a new one in the next frame
            }
           
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_activeCoroutines.ContainsKey(other))
        {
            var enemy = other.GetComponent<Enemy>();
            if (!enemy.IsDead())
            {
                enemy._currentState = Enemy.EnemyState.Slowed;
            
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
    
    IEnumerator SpawnDecals(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); 
            GameObject iceDecal = ObjectPoolManager.Instance._decalsPool.Get(
                _currentStats.StatusEffectPrefab.gameObject, 
                transform.position + new Vector3(0, 0.1f, 0), 
                ObjectPoolManager.PoolType.Decals);
            iceDecal.transform.position = transform.position + new Vector3(0, 0.1f, 0);
            IceBehavior iceBehavior = iceDecal.GetComponent<IceBehavior>();
            iceBehavior.Initialize(_currentStats);
            CoroutineManager.Instance.StartManagedCoroutine(EnterPoolAfterDelay(_currentStats.DecalLifetime, iceDecal));
        }
    }
    
    IEnumerator EnterPoolAfterDelay(float delay, GameObject obj)
    {
        yield return new WaitForSeconds(delay); 
        ObjectPoolManager.Instance._decalsPool.Release(obj);
    }
  

}

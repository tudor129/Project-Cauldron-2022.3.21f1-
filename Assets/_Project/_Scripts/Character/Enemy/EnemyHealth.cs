using MoreMountains.Feedbacks;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class EnemyHealth : Health
{
    [SerializeField] EnemyHealthBarUI _healthBarUI;
    
    EnemyVisualEffects _visualEffects;
    AIPath _aiPath;
    FollowerEntity _followerEntity;
    Coroutine _staggerRoutine;
    Coroutine _dotRoutine;
    SpellInstance _spellInstance;
    
    bool _hasDoTEffect;
    bool _returningToPool;

    protected override void Awake()
    {
        base.Awake();
        //_aiPath = GetComponent<AIPath>();
        _followerEntity = GetComponent<FollowerEntity>();
        _visualEffects = GetComponent<EnemyVisualEffects>();
        _animatorManager = GetComponent<CharacterAnimatorManager>();
        _healthBarUI.Initialize(this);
       
        UpdateHealthUI();
    }
    
    protected override void Start()
    {
        base.Start();
      
    }

    void OnEnable()
    {
       
       _followerEntity.enabled = true;
        _hasDoTEffect = false;
    }
    void OnDisable()
    {
        _isAlive = false;
    }


    public override void TakeDamage(int amount, bool isCritical, Spell.Stats spellInfo, bool isDirectDamage = true)
    {
        if (!IsDead())
        {
            base.TakeDamage(amount, isCritical, spellInfo, true);
            UpdateHealthUI();
            HandleDamageNumbers(amount, isCritical);

            if (isDirectDamage)
            {
                ApplyStaggerEffect(spellInfo.StaggerDuration);
            } 
        }
        
        
    }
    
    void ApplyStaggerEffect(float duration)
    {
        if (_staggerRoutine != null)
        {
            StopCoroutine(_staggerRoutine);
        }
        
        _staggerRoutine = StartCoroutine(StaggerRoutine(duration));
    }
    
    IEnumerator StaggerRoutine(float duration)
    {
        //_aiPath.canMove = false;
        //_followerEntity.maxSpeed = 0;
        _animatorManager.StopAttackAnimation();
        _animatorManager.StopWalkingAnimation();
        yield return new WaitForSeconds(duration);
        _animatorManager.PlayWalkingAnimation();
        //_followerEntity.maxSpeed = 2;
        
        //_aiPath.canMove = true;
        if (!_isAlive)
        {
            //_aiPath.canMove = false;
            //_followerEntity.maxSpeed = 0;
            yield break;
        }
    }
    
    public void ApplyDoT(Spell.Stats spellInfo)
    {
        if (_dotRoutine != null)
        {
            StopCoroutine(_dotRoutine);
        }
        _dotRoutine = StartCoroutine(DoTRoutine(
            spellInfo.DamageOverTime, 
            spellInfo.DamageOverTimeDuration, 
            spellInfo.DamageOverTimeInitialDelay, 
            spellInfo.DamageOverTimeInterval, 
            spellInfo));
    }

    IEnumerator DoTRoutine(int damagePerTick, float duration, float initialDelay, float tickInterval, Spell.Stats spellInfo)
    {
        _hasDoTEffect = true;
        
        yield return new WaitForSeconds(initialDelay);
        
        float elapsed = 0;
        tickInterval = spellInfo.DamageOverTimeInterval; // Adjust as needed for frequency of damage application

        while (elapsed < duration)
        {
            elapsed += tickInterval;
            TakeDamage(damagePerTick, false, spellInfo, false); // Assuming non-critical damage
            yield return new WaitForSeconds(tickInterval);
        }
        
        _hasDoTEffect = false;
    }
    
    void UpdateHealthUI()
    {
        HealthEventPayload payload = new HealthEventPayload
        {
            CurrentHealth = _currentHealth,
            EnemyHealthInstance = this
        };

        EventManager<HealthEventPayload>.TriggerEvent(EventKey.UPDATE_ENEMY_HEALTH, payload);
    }
   
    void HandleDamageNumbers(int amount, bool isCritical)
    {
        if (isCritical)
        {
            DamageNumberSpawner.Instance.SpawnCriticalDamageNumber(transform.position + new Vector3(0, 2.5f, 0), amount);
        }
        else
        {
            DamageNumberSpawner.Instance.SpawnDamageNumber(transform.position + new Vector3(0, 2, 0), amount);
        }
    }
    

    protected override void Die()
    {   
        
        base.Die();
        _isAlive = false;
        //_aiPath.canMove = false;
        _followerEntity.enabled = false;
        _animatorManager._animator.speed = 1.5f;
        
        
        if (_staggerRoutine != null)
        {
            StopCoroutine(_staggerRoutine); 
        }
        if (_dotRoutine != null)
        {
            StopCoroutine(_dotRoutine);
        }

        _followerEntity.enabled = false;
        //_aiPath.canMove = false;
        
       
        
        StartCoroutine(DeactivateEffectsAfterDelay(3));
        
        var enemyComponent = GetComponent<Enemy>();
        if (enemyComponent == null)
        {
            return;
        }
        var EnemyData = enemyComponent.EnemyData;
        if (EnemyData == null)
        {
            return;
        }
        
        Enemy.ActiveEnemies.Remove(enemyComponent);
        
        LootManager.Instance.DropLoot(transform, EnemyData);
        
        HandleReturnToPool();
    }
    
    void HandleReturnToPool()
    {
        _animatorManager.StopWalkingAnimation();
        _returningToPool = true;
        StartCoroutine(DelayedReturn(gameObject, 5f));
    }
    
    IEnumerator DelayedReturn(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        StopAllCoroutines();
        
        //ObjectPoolManager.Instance.ReturnObjectToPool(obj);
        EnemySpawner.Instance.ReleaseObjectFromPool(obj);
       
        //ObjectPoolManager.SetParentObject(ObjectPoolManager.PoolType.Enemy);
        Respawn();
        _returningToPool = false;
    }

    IEnumerator DeactivateEffectsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //_fireEffect.gameObject.SetActive(false);
    }
    
    public bool HasDotEffect()
    {
        return _hasDoTEffect;
    }

    
}

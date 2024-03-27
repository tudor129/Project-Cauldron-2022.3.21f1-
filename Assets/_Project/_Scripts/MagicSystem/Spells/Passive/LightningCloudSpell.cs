/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LightningCloudSpell : BaseAutoCastSpell
{
    [FormerlySerializedAs("LightningCloudSO")] public LightningCloudData lightningCloudData;
    
    List<IAttackable> _attackablesInRadius = new List<IAttackable>();
   
    float _timer;
    float _enemyCheckCounter;
    
    protected override void Awake()
    {
        _attackablesInRadius.Clear();
        base.Awake();
        _part = GetComponent<ParticleSystem>();
        Initialize(lightningCloudData, _player, PlayerData);
        _spawnPosition = GameManager.Instance._playerCastPoint.transform.position;
        _spawnRotation = transform.rotation;
    }
    
    

    protected override void Start()
    {
        _attackablesInRadius.Clear();
        transform.position = _spawnPosition;
    }


    protected override void Update()
    {
        _timer -= Time.deltaTime;
        _enemyCheckCounter -= Time.deltaTime;

        if (_timer <= 0)
        {
            Vector3 targetPosition;

            if (_enemyCheckCounter <= 0)
            {
                targetPosition = FindNearestEnemyPosition();
                _enemyCheckCounter = lightningCloudData.EnemyCheckTimer;
            }
            else
            {
                // Use random wandering behavior
                Vector3 randomDirection = Random.insideUnitCircle.normalized * lightningCloudData.WanderRadius;
                targetPosition = _player.transform.position + new Vector3(randomDirection.x, transform.position.y, randomDirection.y);
            }

            StartCoroutine(MoveToTargetPosition(targetPosition));
            _timer = lightningCloudData.WanderTimer;
        }
    }
    
    Vector3 FindNearestEnemyPosition()
    {
        float minDistance = float.MaxValue;
        Vector3 nearestEnemyPosition = transform.position;
        bool enemyFound = false;

        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, lightningCloudData.EnemyDetectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(_player.transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemyPosition = enemy.transform.position;
                    enemyFound = true;
                }
            }
        }

        // Return the position of the nearest enemy or a random position around the player if no enemy is found
        return enemyFound ? nearestEnemyPosition : GetRandomPositionAroundPlayer();
    }
    
    Vector3 GetRandomPositionAroundPlayer()
    {
        Vector3 targetPosition;
        // Use random wandering behavior
        Vector3 randomDirection = Random.insideUnitCircle.normalized * lightningCloudData.WanderRadius;
        targetPosition = _player.transform.position + new Vector3(randomDirection.x, transform.position.y, randomDirection.y);
        
        return targetPosition;
    }

    
    IEnumerator MoveToTargetPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        float moveTime = 1.0f; // Adjust this for the speed of the movement

        Vector3 startingPosition = transform.position;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches the exact target position
        transform.position = targetPosition;
    }
    

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
        
        int numCollisionEvents = _part.GetCollisionEvents(other, _collisionEvents);     
        for (int i = 0; i < numCollisionEvents; i++)
        {
            // Making an overlap sphere to add all attackables in range to a list
            Collider[] collidersInSphere = Physics.OverlapSphere(_collisionEvents[i].intersection, lightningCloudData.BaseSpellRadius);
            
            // Then we iterate through that list and apply damage to all of them
            foreach (var collider in collidersInSphere)
            {
                if (collider.CompareTag("Player"))
                {
                    continue;
                }
                
                collider.TryGetComponent(out IAttackable attackable);

                if (attackable != null && !_attackablesInRadius.Contains(attackable))
                {
                    _attackablesInRadius.Add(attackable);
                }
            }

            List<IAttackable> attackablesToRemove = new List<IAttackable>();
            
            foreach (var attackable in _attackablesInRadius)
            {
                _hitCount++;
                (int damage, bool isCritical) = CalculateAttackDamage(_hitCount, PlayerData.GetCriticalChance(), PlayerData.GetCriticalMultiplier(), lightningCloudData);
                attackable.TakeDamage(damage, isCritical, lightningCloudData);
                attackablesToRemove.Add(attackable);
            }
            foreach (var attackableToRemove in attackablesToRemove)
            {
                _attackablesInRadius.Remove(attackableToRemove);
            }
            _hitCount = 0;
            
            foreach (var effect in _effectsOnCollision)
            {
                var instance = Instantiate(effect, _collisionEvents[i].intersection + _collisionEvents[i].normal * _offset, new Quaternion());
                
                if (!_useWorldSpacePosition) instance.transform.parent = transform;
                if (_useFirePointRotation) { instance.transform.LookAt(transform.position); }
                else if (_rotationOffset != Vector3.zero && _useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(_rotationOffset); }
                else
                {
                    instance.transform.LookAt(_collisionEvents[i].intersection + _collisionEvents[i].normal);
                    instance.transform.rotation *= Quaternion.Euler(_rotationOffset);
                }
                Destroy(instance, _destroyTimeDelay);
            }
        }
        if (_destoyMainEffect)
        {
            Destroy(gameObject, _destroyTimeDelay + 0.5f);
        }
    }


    protected override (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, SpellData damageType)
    {
        // Random range between base damage and base damage + 15% of base damage
        lightningCloudData.DamageOutput = Random.Range(lightningCloudData.BaseDamage, lightningCloudData.BaseDamage + (int)(lightningCloudData.BaseDamage * 0.15f));
        
        
        bool isCriticalHit = false;
        
        // Calculate critical hit
        float roll = UnityEngine.Random.value; // Get a random value between 0.0 and 1.0
        if (roll <= critChance) // If the random value is less than or equal to the crit chance, a critical hit occurs
        {
            lightningCloudData.DamageOutput = (int)(lightningCloudData.DamageOutput * critMultiplier); // Multiply the damage by the critical hit multiplier
            isCriticalHit = true;
        }
        return (lightningCloudData.DamageOutput, isCriticalHit);
    }
}
*/

using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// All projectile spells will inherit from BaseProjectileSpell
public class Fireball : Spell
{
    
    Camera _mainCamera;
    int _currentAttackCount;
    float _currentAttackInterval;
    protected bool _readyForAttack = true;
    
    
    ProjectileBehavior _projectileBehaviorGameObject;
    IEnumerator _despawnAfterdelay;
    MMF_Player _feedback;
    
    float _currentCooldown;

    protected override void Awake()
    {
        base.Awake();
        //_currentStats = GetStats();
        
    }

    protected override void Start()
    {
        base.Start();
        _currentAttackInterval = _currentStats.ProjectileInteval;
        _feedback = GetComponentInChildren<MMF_Player>();
    }
   
    protected override void Update()
    {
        _currentCooldown -= Time.deltaTime;
        
        if (_currentCooldown <= 0f)
        {
            _currentAttackCount = _currentStats.NumberOfAttacks; 
            _currentAttackInterval = 0f; // Start the first attack immediately
            _currentCooldown = _currentStats.Cooldown; // Reset the cooldown
        }

        if (_currentAttackCount > 0 && _currentAttackInterval <= 0f)
        {
            Attack(_currentAttackCount);
            if (_currentStats.SpawnsProjectilesSequentially)
            {
                _currentAttackCount--; // Decrement the attack count
                _currentAttackInterval = _currentStats.ProjectileInteval; // Reset the interval for the next attack
            }
            else
            {
                _currentAttackCount -= _currentStats.NumberOfAttacks; // Subtract number of attacks from the attack count
                _currentAttackInterval = _currentStats.ProjectileInteval;
            }
        }

        if (_currentAttackInterval > 0)
        {
            _currentAttackInterval -= Time.deltaTime;
        }
    }
    
    IEnumerator DespawnAfterDelay(float delay, GameObject gameObject)
    {
        yield return new WaitForSeconds(delay); // wait for 'delay' seconds
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject); // then call your function to return the game object to the pool
    }
    
    public override bool CanAttack()
    {
        if (_currentAttackCount > 0) return true;
        return _currentCooldown <= 0;
    }
    
    protected override bool Attack(int attackCount = 1)
    {
        if (!_currentStats.ProjectilePrefab || !CanAttack()) 
        {
            Debug.LogWarning($"Projectile prefab has not been set for {name} or cannot attack.");
            _currentCooldown = _currentStats.Cooldown;
            return false;
            
            
        }
        
        //_feedback.PlayFeedbacks();

        if (_currentStats.SpawnsProjectilesSequentially)
        {
            Vector3 basePosition = _player.transform.position + Vector3.up; // Base spawn position, adjusted upward
            float angleStep = _currentStats.AngleStep / attackCount; // Divide a circle into equal parts based on attackCount, basically the "cone" in front of the player
            float radius = _currentStats.SpreadRadius; // Define the radius of the circle around the player where spells will spawn, not to be confused with SpellRadius
            bool isAttackCountEven = attackCount % 2 == 0;
            float middleIndex = isAttackCountEven ? (attackCount * 0.5f) - 0.5f : (attackCount - 1) * 0.5f;
            float offsetAngle = middleIndex * angleStep; // This is needed to make the middle spell spawn in front of the player
            
            for (int i = 0; i < attackCount; i++)
            {
                Vector3 direction = Quaternion.Euler(0, angleStep * i - offsetAngle, 0) * GetDirection();
                Vector3 spawnPosition = basePosition + direction * radius; // Calculate the spawn position
                
                //SpellBehavior prefab = Instantiate(_currentStats.SpellPrefab, spawnPosition, Quaternion.LookRotation(direction));
                
                ProjectileBehavior prefab = ObjectPoolManager.Instance.SpawnObject(
                    _currentStats.ProjectilePrefab, 
                    spawnPosition, 
                    Quaternion.LookRotation(direction), 
                    ObjectPoolManager.PoolType.Projectiles);
                
                prefab.Initialize(this, _feedback);


                // Remember to fix this
                if (!_currentStats.IsPassThrough)
                {
                    Debug.Log("Fireball is not pass through");
                    StartCoroutine(DespawnAfterDelay(_currentStats.Lifetime, prefab.gameObject));
                }
                
              
                
                if (_currentStats.CastSound != null)
                {
                    SoundFXManager.Instance.PlaySoundFXClip(_currentStats.CastSound, prefab.transform, 0.5f);
                }
                else if (_currentStats.CastSounds.Length > 0)
                {
                    SoundFXManager.Instance.PlaySoundFXClipWithVariation(_currentStats.CastSounds, prefab.transform, 0.5f);
                }
            }
        }
        else
        {
            Vector3 basePosition = _player.transform.position + Vector3.up; // Base spawn position, adjusted upward
            float angleStep = _currentStats.AngleStep / attackCount; // Divide a circle into equal parts based on attackCount, basically the "cone" in front of the player
            float radius = _currentStats.SpreadRadius; // Define the radius of the circle around the player where spells will spawn, not to be confused with SpellRadius
            bool isAttackCountEven = attackCount % 2 == 0;
            float middleIndex = isAttackCountEven ? (attackCount * 0.5f) - 0.5f : (attackCount - 1) * 0.5f;
            float offsetAngle = middleIndex * angleStep; // This is needed to make the middle spell spawn in front of the player
            

            for (int i = 0; i < attackCount; i++)
            {
                Vector3 direction = Quaternion.Euler(0, angleStep * i - offsetAngle, 0) * GetDirection();
                Vector3 spawnPosition = basePosition + direction * radius; // Calculate the spawn position

                //ProjectileBehavior prefab1 = Instantiate(_currentStats.SpellPrefab, spawnPosition, Quaternion.LookRotation(direction));
                
                ProjectileBehavior prefab1 = ObjectPoolManager.Instance.SpawnObject(
                    _currentStats.ProjectilePrefab, 
                    spawnPosition, 
                    Quaternion.LookRotation(direction), 
                    ObjectPoolManager.PoolType.Projectiles);
                
                prefab1.Initialize(this, _feedback);
                
                StartCoroutine(DespawnAfterDelay(_currentStats.Lifetime, prefab1.gameObject));
                
                // prefab1.spell = this;
                // prefab1.spell.GetStats();
                
                if (_currentStats.CastSound != null)
                {
                    SoundFXManager.Instance.PlaySoundFXClip(_currentStats.CastSound, prefab1.transform, 0.5f);
                }
                else if (_currentStats.CastSounds.Length > 0)
                {
                    SoundFXManager.Instance.PlaySoundFXClipWithVariation(_currentStats.CastSounds, prefab1.transform, 0.5f);
                }
                
            }
        }
      
        _currentCooldown = _currentStats.Cooldown;
        return true;
        
        Vector3 GetDirection()
        {
            if (_currentStats.ShootBackwards)
            {
                return -_player.transform.forward;
            }
            else
            {
                return _player.transform.forward;
            }
        }
    }
}

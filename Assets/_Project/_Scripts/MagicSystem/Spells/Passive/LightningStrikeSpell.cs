using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LightningStrikeSpell : Spell
{
    protected override void Update()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0f) //Once the cooldown becomes 0, attack
        {
            Attack(_currentStats.NumberOfAttacks);
        }
       
    }
    
    protected override bool CanAttack()
    {
        return _currentCooldown <= 0;
    }
    
    protected override bool Attack(int attackCount = 1)
    {
        // If no projectile prefab is assigned, leave a warning message.
        if (!_currentStats.ProjectilePrefab)
        {
            Debug.LogWarning(string.Format("Projectile prefab has not been set for {0}", name));
            _currentCooldown = _currentStats.Cooldown;
            return false;
        }

        // Can we attack?
        if (!CanAttack()) return false;

       

        // And spawn a copy of the projectile.
        BaseSpellBehavior prefab = Instantiate(_currentStats.ProjectilePrefab, _player.transform.position + Vector3.up, Quaternion.identity);
        
        prefab.spell = this;
        
            
        

        // Reset the cooldown only if this attack was triggered by cooldown.
        if (_currentCooldown <= 0)
            _currentCooldown += _currentStats.Cooldown;

        attackCount--;

        // Do we perform another attack?
        if (attackCount > 0)
        {
            _hitCount = attackCount;
            
        }

        return true;
    }
    
    /*[FormerlySerializedAs("LightningStrikeSO")] public LightningStrikeData lightningStrikeData;
    
    List<Collider> _enemyColliders = new List<Collider>();
    
    float _enemyDetectionRadius = 20f;
    float _range = 10f;

    protected override void Awake()
    {
        _enemyColliders.Clear();
        base.Awake();
        _part = GetComponent<ParticleSystem>();
        Initialize(lightningStrikeData, _player, PlayerData);
        _spawnPosition = GameManager.Instance._playerCastPoint.transform.position;
        _spawnRotation = transform.rotation;
    }

    protected override void Start()
    {
        //_enemyColliders.Clear();
        Vector3? randomEnemyPosition = FindRandomEnemyPosition();
        if (randomEnemyPosition.HasValue)
        {
            transform.position = randomEnemyPosition.Value;
            DealDamage();
        }
        else
        {
            Destroy(gameObject);
        }
        
        Destroy(gameObject, 2);
    }

    protected override void Update()
    {
        
    }
    
    Vector3? FindRandomEnemyPosition()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _enemyDetectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                _enemyColliders.Add(hitCollider);
            }
        }

        if (_enemyColliders.Count > 0)
        {
            // Select a random enemy
            int randomIndex = UnityEngine.Random.Range(0, _enemyColliders.Count);
            return _enemyColliders[randomIndex].transform.position;
        }

        // // Return null if no enemies are found
         return null;
        
    }
    
    void DealDamage()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 4f, Vector3.down, out hit, _range))
        {
            if (hit.collider.gameObject.TryGetComponent(out IAttackable attackable))
            {
                // Apply damage
                (int damage, bool isCritical) = CalculateAttackDamage(_hitCount, PlayerData.GetCriticalChance(), PlayerData.GetCriticalMultiplier(), lightningStrikeData);
                attackable.TakeDamage(damage, isCritical, lightningStrikeData);
            }
        }
    }


    protected override (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, SpellData damageType)
    {
        // Random range between base damage and base damage + 15% of base damage
        lightningStrikeData.DamageOutput = Random.Range(lightningStrikeData.BaseDamage, lightningStrikeData.BaseDamage + (int)(lightningStrikeData.BaseDamage * 0.15f));
        
        
        bool isCriticalHit = false;
        
        // Calculate critical hit
        float roll = UnityEngine.Random.value; // Get a random value between 0.0 and 1.0
        if (roll <= critChance) // If the random value is less than or equal to the crit chance, a critical hit occurs
        {
            lightningStrikeData.DamageOutput = (int)(lightningStrikeData.DamageOutput * critMultiplier); // Multiply the damage by the critical hit multiplier
            isCriticalHit = true;
        }
        return (lightningStrikeData.DamageOutput, isCriticalHit);
    }*/
}
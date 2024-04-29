using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class IceTornado : Spell
{
    protected override void Update()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0f) //Once the cooldown becomes 0, attack
        {
            Attack(_currentStats.NumberOfAttacks);
        }
    }
    
     protected override bool Attack(int attackCount = 1)
    {
        // If no projectile prefab is assigned, leave a warning message.
        if (!_currentStats.SpellPrefab)
        {
            Debug.LogWarning(string.Format("Spell prefab has not been set for {0}", name));
            _currentCooldown = _currentStats.Cooldown;
            return false;
        }

        // Can we attack?
        if (!CanAttack()) return false;
       
        
        if (_currentStats.SpawnsProjectilesSequentially)
        {
            Vector3 basePosition = _player.transform.position; 
            float angleStep = _currentStats.AngleStep / attackCount; // Divide a circle into equal parts based on attackCount, basically the "cone" in front of the player
            float radius = _currentStats.SpreadRadius; // Define the radius of the circle around the player where spells will spawn, not to be confused with SpellRadius
            bool isAttackCountEven = attackCount % 2 == 0;
            float middleIndex = isAttackCountEven ? (attackCount * 0.5f) - 0.5f : (attackCount - 1) * 0.5f;
            float offsetAngle = middleIndex * angleStep; // This is needed to make the middle spell spawn in front of the player
            
            for (int i = 0; i < attackCount; i++)
            {
                Vector3 direction = Quaternion.Euler(0, angleStep * i - offsetAngle, 0) * _player.transform.forward;
                Vector3 spawnPosition = basePosition + direction * radius; // Calculate the spawn position
                
                //SpellBehavior prefab = Instantiate(_currentStats.SpellPrefab, spawnPosition, Quaternion.LookRotation(direction));
                
                IceTornadoBehavior prefab = ObjectPoolManager.Instance.SpawnObject<IceTornadoBehavior>(
                    _currentStats.SpellPrefab.gameObject, 
                    spawnPosition, 
                    Quaternion.LookRotation(direction), 
                    ObjectPoolManager.PoolType.GameObject);
                
                prefab.Initialize(this, null);
                
                CoroutineManager.Instance.StartManagedCoroutine(ReturnToPoolAfterDelay(_currentStats.Lifetime, prefab.gameObject));
                
                //Destroy(prefab.gameObject, _currentStats.Lifetime);
            }
        }

        // Reset the cooldown only if this attack was triggered by cooldown.
        if (_currentCooldown <= 0)
            _currentCooldown += _currentStats.Cooldown;

        return true;
    }
}

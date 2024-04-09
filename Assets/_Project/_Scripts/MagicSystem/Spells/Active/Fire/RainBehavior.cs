using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class RainBehavior : BaseSpellBehavior
{
    [SerializeField] GameObject _lavaPrefab;
    
    List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();
    
    ParticleSystem _particleSystem;

    int _collisionCount;
    protected override void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(this._particleSystem, other, _collisionEvents);

        Debug.Log("Number of collision events: " + numCollisionEvents);
        if (other.CompareTag("Ground"))
        {

            for (int i = 0; i < numCollisionEvents; i++)
            {
                if (_collisionCount % 2 == 0) // Check if the counter is even
                {
                    Vector3 hitPosition = _collisionEvents[i].intersection;

                    GameObject lavaObject = Instantiate(_lavaPrefab, hitPosition, Quaternion.identity);
                    
                    // This is called from LavaBehavior, not it's parent class
                    lavaObject.GetComponent<LavaBehavior>().Initialize(this, _currentStats);
                    
                    Destroy(lavaObject, 10f);
                }
                _collisionCount++;
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            HandleDealDamage(_currentStats, other);

            if (_currentStats.IsFire)
            {
                other.transform.Find("Spell_Light_6_LWRP").gameObject.SetActive(true);
            }
            
            GameObject lavaObject = Instantiate(_lavaPrefab, other.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            
            lavaObject.GetComponent<LavaBehavior>().Initialize(this, _currentStats);
            //lavaObject.GetComponent<LavaBehavior>().Initialize();
            
            Destroy(lavaObject, 10f);
            
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Rain collided with player.");
        }
        
    }
    
    void HandleDealDamage(Spell.Stats spellData, GameObject other)
    {
        other.GetComponent<EnemyHealth>().TakeDamage(_currentStats.Damage, false, spellData, true);
    }

    void OnParticleTrigger()
    {
        Debug.Log("Rain triggered.");
    }

}

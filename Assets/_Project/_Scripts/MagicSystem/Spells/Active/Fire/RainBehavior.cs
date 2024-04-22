using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class RainBehavior : BaseSpellBehavior
{
    [SerializeField] MMF_Player _feedback;
    
    [SerializeField] GameObject _lavaPrefab;
    
    List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();
    
    ParticleSystem _particleSystem;

    int _collisionCount;
    protected override void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        //_feedback = FindObjectOfType<MMF_Player>();
    }

    protected override void Start()
    {
        base.Start();
        _collisionCount = 0;
    }
    
    public void Initialize(MMF_Player feedback)
    {
        _feedback = feedback;
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(this._particleSystem, other, _collisionEvents);

        if (other.CompareTag("Ground"))
        {
            for (int i = 0; i < numCollisionEvents; i++)
            {
                if (_collisionCount % 2 == 0) // Check if the counter is even
                {
                    Vector3 hitPosition = _collisionEvents[i].intersection;

                    BaseSpellBehavior lavaObject = ObjectPoolManager.Instance.SpawnObject<BaseSpellBehavior>(
                        _lavaPrefab,
                        hitPosition,
                        Quaternion.identity,
                        ObjectPoolManager.PoolType.Decals);
                    
                    // This is called from LavaBehavior, not it's parent class
                    lavaObject.GetComponent<LavaBehavior>().Initialize(this, _currentStats);
   
                    CoroutineManager.Instance.StartManagedCoroutine(ReturnToPoolAfterDelay(_currentStats.DecalLifetime, lavaObject.gameObject));
                }
                _collisionCount++;
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            HandleDealDamage(_currentStats, other);

            BaseSpellBehavior lavaObject = ObjectPoolManager.Instance.SpawnObject<BaseSpellBehavior>(
                _lavaPrefab,
                other.transform.position + new Vector3(0, 0.1f, 0),
                Quaternion.identity,
                ObjectPoolManager.PoolType.Decals);
            
            lavaObject.GetComponent<LavaBehavior>().Initialize(this, _currentStats);
            
            CoroutineManager.Instance.StartManagedCoroutine(ReturnToPoolAfterDelay(_currentStats.DecalLifetime, lavaObject.gameObject));

            //MMF_Player feedback = Instantiate(_currentStats.Feedback, other.transform.position, Quaternion.identity);
            _feedback.PlayFeedbacks();
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Rain collided with player.");
        }
        
    }
    
    // IEnumerator ReturnToPoolAfterDelay(float delay, GameObject objectToReturn)
    // {
    //     yield return new WaitForSeconds(delay);
    //     ObjectPoolManager.Instance.ReturnObjectToPool(objectToReturn);
    // }
    
    void HandleDealDamage(Spell.Stats spellData, GameObject other)
    {
        other.GetComponent<EnemyHealth>().TakeDamage(_currentStats.Damage, false, spellData, true);
    }

    void OnParticleTrigger()
    {
        Debug.Log("Rain triggered.");
    }

}

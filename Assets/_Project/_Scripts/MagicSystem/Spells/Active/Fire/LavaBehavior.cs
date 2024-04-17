using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehavior : BaseSpellBehavior
{
    SphereCollider _sphereCollider;

    protected override void Awake()
    {
        base.Awake();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    protected override void Start()
    {
        
    }
    
    public void Initialize(RainBehavior caller, Spell.Stats stats)
    {
        _currentStats = stats;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            bool effectExists = other.gameObject.transform.Find(_currentStats.StatusEffectPrefab.name);
            if (effectExists)
            {
                return;
            }
            other.GetComponent<EnemyHealth>().ApplyDoTEffect(_currentStats);
            GameObject lavaObject = ObjectPoolManager.Instance.SpawnStatusEffect(
                _currentStats.StatusEffectPrefab,
                 other.gameObject,
                other.transform.position + new Vector3(0, 0.1f, 0),
                Quaternion.identity,
                ObjectPoolManager.PoolType.StatusEffects);
            
            CoroutineManager.Instance.StartManagedCoroutine(ReturnToPoolAfterDelay(_currentStats.DamageOverTimeDuration, lavaObject));
        }
    }
    
    IEnumerator ReturnToPoolAfterDelay(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);
        ObjectPoolManager.Instance.ReturnStatusEffectToPool(objectToReturn);
    }
}

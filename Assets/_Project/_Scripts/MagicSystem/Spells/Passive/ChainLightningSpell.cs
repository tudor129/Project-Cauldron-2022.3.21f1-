/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ChainLightningSpell : BaseAutoCastSpell
{
    [FormerlySerializedAs("ChainLightningSO")] public ChainLightningData chainLightningData;
    
    List<Collider> _enemyColliders = new List<Collider>();

    int _chainCount;
    int _singleSpawns;

    float _closestEnemyDistance;

    GameObject _startTarget;
    GameObject _endTarget;
    
    IEnumerator _lerpRadiusCoroutine;
    
    protected override void Awake()
    {
        _enemyColliders.Clear();
        base.Awake();
        _part = GetComponent<ParticleSystem>();
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;

        Initialize(chainLightningData, _player, PlayerData);
        _spawnPosition = GameManager.Instance._playerCastPoint.transform.position;
        _chainCount = chainLightningData.ChainCount;
        _startTarget = gameObject;
        _lerpRadiusCoroutine = LerpRadius(0, chainLightningData.ChainRange, chainLightningData.ChainSpeed);
    }
    
    protected override void Start()
    {
        if (_chainCount <= 0)
        {
            Destroy(gameObject);
        }
        
        FindClosestEnemy();
        
        _singleSpawns = 1;

        Destroy(gameObject, 3f);
    }
    
    // Use this for ensuring the created object is indeed a WildfireProjectileSpell and set its initial target
    public void SetInitialTarget(GameObject target)
    {
        _startTarget = target;
    }
 
    IEnumerator LerpRadius(float startRadius, float endRadius, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            _sphereCollider.radius = Mathf.Lerp(startRadius, endRadius, timeElapsed / duration);
            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }
    }
    
   
    
    void FindClosestEnemy()
    {
       StartCoroutine(_lerpRadiusCoroutine);
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == null)
        {
            Destroy(gameObject);
        }
        int enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        
        
        if (other.gameObject.layer != enemyLayerIndex)
        {
            return;
        }
        
        if (_singleSpawns == 0)
        {
            Destroy(gameObject);
        }
        
        if (_chainCount == 0)
        {
            Destroy(gameObject);
        }
    
        // Check if the object is on the "Enemy" layer and if the chain count is more than 0
        if (other.gameObject.layer == enemyLayerIndex && !other.GetComponentInChildren<LightningStruckEffect>())
        {
            if (_singleSpawns != 0 && _chainCount  > 0)
            {
                
                int newChainCount = _chainCount;
                _endTarget = other.gameObject;
                
                // Calculate the direction from the spell to the other collider
                Vector3 directionToTarget = _endTarget.transform.position - transform.position;
                Vector3 dir = _endTarget.transform.forward;
        
                // Create a rotation that looks along the directionToTarget
                Quaternion rotationTowardsTarget = Quaternion.LookRotation(dir);

                newChainCount--;
                
                var bolt = Instantiate(chainLightningData.SpellProjectileFX, other.gameObject.transform.position, rotationTowardsTarget);
                bolt.GetComponent<ChainLightningSpell>()._chainCount = newChainCount;
                
                
                
                _singleSpawns--;
              
                // This is also basically a cooldown for how often the spell can hit the same enemy
                var struckEffect = Instantiate(chainLightningData.BeenStruckEffect, other.gameObject.transform);
                _sphereCollider.enabled = false;
                
                _part.Play();
                
                var emitParams = new ParticleSystem.EmitParams();
                
                emitParams.position = _startTarget.transform.position + Vector3.up;
                _part.Emit(emitParams, 1);
                emitParams.position = _endTarget.transform.position + Vector3.up;
                _part.Emit(emitParams, 1);
                // emitParams.position = (_startTarget.transform.position + _endTarget.transform.position) / 2 + Vector3.up;
                // _part.Emit(emitParams, 1);
                
                
            
                DealDamage(other.GetComponent<IAttackable>());
                
                Debug.Log(_chainCount);
                
                Destroy(gameObject, 0.5f);
            }
        }
        
        if (_chainCount <= 0)
        {
            Destroy(gameObject);
        }
        
        
       
    }
   
    void DealDamage(IAttackable attackable)
    {
        if (attackable == null)
        {
            return;
        }
        (int damage, bool isCritical) = CalculateAttackDamage(_hitCount, PlayerData.GetCriticalChance(), PlayerData.GetCriticalMultiplier(), chainLightningData);
        attackable.TakeDamage(damage, isCritical, chainLightningData);
    }

    protected override (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, SpellData damageType)
    {
        // Random range between base damage and base damage + 15% of base damage
        chainLightningData.DamageOutput = Random.Range(chainLightningData.BaseDamage, chainLightningData.BaseDamage + (int)(chainLightningData.BaseDamage * 0.15f));
        
        
        bool isCriticalHit = false;
        
        // Calculate critical hit
        float roll = UnityEngine.Random.value; // Get a random value between 0.0 and 1.0
        if (roll <= critChance) // If the random value is less than or equal to the crit chance, a critical hit occurs
        {
            chainLightningData.DamageOutput = (int)(chainLightningData.DamageOutput * critMultiplier); // Multiply the damage by the critical hit multiplier
            isCriticalHit = true;
        }
        return (chainLightningData.DamageOutput, isCriticalHit);
    }
}
*/

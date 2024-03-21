using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : SpellBehavior
{
    [SerializeField] protected GameObject flash;
    [SerializeField] protected GameObject[] Detached;
      
    protected List<IAttackable> _attackablesInRadius = new List<IAttackable>();
    protected GameObject _flashInstance;
    protected int _hitCount;
    
    
    SphereCollider _sphereCollider;
    Rigidbody _rigidbody;
    Vector3 _spawnPosition;
    Quaternion _spawnRotation;
    
    bool _isInitialized = false;
    
    protected override void Awake()
    {
        base.Awake();
    }
    
    void OnEnable()
    {
        _attackablesInRadius.Clear();
    }

    protected override void Start()
    {
        _attackablesInRadius.Clear();
        base.Start();
        
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        _part= GetComponent<ParticleSystem>();
        
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.drag = 0.2f;

        if (_currentStats.HasFlash)
        {
            HandleFlashEffect();
        }

        if (_currentStats.TravelingSound != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(_currentStats.TravelingSound, transform, 0.2f);
        }
        //StartCoroutine(DespawnAfterDelay(_currentStats.ProjectileLifetime));
        
        //Destroy(gameObject, _currentStats.ProjectileLifetime);
    }
    
    public void Initialize(Spell spell)
    {
        if (_isInitialized) return;  // Prevent re-initialization

        
        this.spell = spell;
        _currentStats = spell.GetStats();
        _isInitialized = true;

        

        // Perform any operations previously in Awake or Start that depend on initialization here
        PostInitialization();
    }

    void PostInitialization()
    {
        //StartCoroutine(DespawnAfterDelay(_currentStats.ProjectileLifetime));
    }

   

    IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // wait for 'delay' seconds
        Debug.Log("In DespawnAfterDelay ProjectileLifetime is: " + _currentStats.ProjectileLifetime);
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject); // then call your function to return the game object to the pool
    }


    protected virtual void FixedUpdate()
    {
        if (_currentStats.Speed > 0)
        {
            transform.Translate(Vector3.forward * _currentStats.Speed * Time.deltaTime, Space.Self);
        }
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Spell") || other.CompareTag("Loot"))
        {
            return;
        }

        Vector3 impactPoint = HandleImpactPoint(other);
        

        HandleDealDamage(_currentStats);
        
        //HandleProjectileImpactEffect(impactPoint);
        
        if (_currentStats.ImpactSound != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(_currentStats.ImpactSound, transform, 0.1f);
        }

        HandleProjectileTrailRemoval();

        if (!_currentStats.IsPassThrough)
        {
            //Destroy(gameObject);
            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
        }
        
        _attackablesInRadius.Clear();
    }
    
    // The method below is called in HandleDealDamage()
    protected virtual void TriggerVisualEffectsOnHit(IAttackable target, Spell.Stats spellInfo)
    {
        EnemyHealth enemyHealth = target as EnemyHealth;
        if (enemyHealth != null)
        {
            EnemyVisualEffects visualEffects = enemyHealth.GetComponent<EnemyVisualEffects>();
            if (visualEffects != null)
            {
                visualEffects.HandleMaterialSwap();
                visualEffects.HandleSpellEffect(spellInfo);
            }
        }
    }

    /// <summary>
    /// Handles the impact point of a collider and returns the impact point.
    /// </summary>
    /// <param name="other">The collider to handle the impact point of.</param>
    /// <returns>The impact point.</returns>
    protected virtual Vector3 HandleImpactPoint(Collider other)
    {
        Vector3 impactPoint = other.transform.position;
        float radius = _currentStats.SpellRadius;

        Collider[] collidersInRadius = Physics.OverlapSphere(impactPoint, radius);
      
        
        foreach (var collider in collidersInRadius)
        {
            if (collider.CompareTag("Player"))
            {
                continue;
            }

            collider.TryGetComponent(out IAttackable attackable);
            

            if (attackable != null && !_attackablesInRadius.Contains(attackable))
            {
                if (_currentStats.SpellRadius <= 2)
                {
                    HandleProjectileImpactEffect(collider.transform.position);
                }
                else if (_currentStats.SpellRadius > 2)
                {
                    HandleProjectileImpactEffect(impactPoint);
                }
                
                _attackablesInRadius.Add(attackable);
            }
        }
        return impactPoint;
    }
    /// <summary>
    /// Handles dealing damage to attackable objects within a certain radius.
    /// </summary>
    /// <remarks>
    /// This method iterates through a list of attackable objects within a radius and deals damage to them if they are active.
    /// It also triggers visual effects upon hit and may apply damage over time effect based on the specific spell used.
    /// </remarks>
    protected virtual void HandleDealDamage(Spell.Stats spellData)
    {
        for (int i = _attackablesInRadius.Count - 1; i >= 0; i--)
        {
            IAttackable attackable = _attackablesInRadius[i];

            if (attackable != null && attackable.IsActive())
            {
                _hitCount++;
                (int damage, bool isCritical) = CalculateActiveSpellDamage(_hitCount, 0.1f, 3, spellData);
                attackable.TakeDamage(damage, isCritical, spellData);
                
                TriggerVisualEffectsOnHit(attackable, spellData);
                
                if (attackable is EnemyHealth enemyHealth && attackable.IsActive())
                {
                    if (ShouldApplyDoT(spellData, enemyHealth))
                    {
                        enemyHealth.ApplyDoTEffect(spellData);
                    }
                }

                if (isCritical) 
                {
                    // Apply audio effects here
                    // Apply visual effects here
                }
            }
            else
            {
                _attackablesInRadius.RemoveAt(i);
            }
        }
    }
    
    /// <summary>
    /// Determines whether a Damage over Time (DoT) effect should be applied based on the specified spell information.
    /// </summary>
    /// <param name="spellInfo">The spell information.</param>
    /// <returns>Returns true if a DoT effect should be applied, false otherwise.</returns>
    bool ShouldApplyDoT(Spell.Stats spellInfo, EnemyHealth enemyHealth)
    {
        return spellInfo.DoesDoT && !enemyHealth.HasDotEffect();
    }
    /// Handles the impact effect of a projectile at a given impact point.
    /// @param impactPoint The point where the projectile impacted.
    /// 
    protected virtual void HandleProjectileImpactEffect(Vector3 impactPoint)
    {
        Spell.Stats spellInfo = spell.GetStats();
        // Spawn hit effect on trigger
        if (spellInfo.SpellImpactPrefab != null)
        {
            SpellImpactBehavior hitInstance = ObjectPoolManager.Instance.SpawnObject(
                _currentStats.SpellImpactPrefab, 
                impactPoint + _currentStats.ImpactOffset, 
                Quaternion.identity, 
                ObjectPoolManager.PoolType.ImpactHits);
            
            
            hitInstance.spell = spell;
        }
        
        // Alternative to the above code
        /*for (int i = _attackablesInRadius.Count - 1; i >= 0; i--)
        {
            IAttackable attackable = _attackablesInRadius[i];

            if (attackable != null)
            {
                var hitInstance = Instantiate(hit, impactPoint, Quaternion.identity);
                hitInstance.transform.position = ((MonoBehaviour)attackable).transform.position;
                
                var hitPs = hitInstance.GetComponent<ParticleSystem>();

                if (hitPs != null)
                {
                    Destroy(_flashInstance);
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                    Destroy(_flashInstance);
                }

            }
            else
            {
                _attackablesInRadius.RemoveAt(i);
            }
        }
        _attackablesInRadius.Clear();*/

        
    }
    protected virtual void HandleProjectileTrailRemoval()
    {
        if (_currentStats.IsPassThrough)
        {
            return;
        }
        // Removing trail from the projectile on trigger enter or smooth removing. Detached elements must have "AutoDestroying script"
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                Debug.Log("Detached prefab is not null");
                //detachedPrefab.transform.parent = null;
                //Destroy(detachedPrefab, 1);
                //StartCoroutine(DespawnAfterDelay())
            }
        }
    }

    protected virtual void HandleFlashEffect()
    {
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            _flashInstance = flashInstance;

            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                //Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                //Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        //Destroy(gameObject, _currentStats.ProjectileLifetime);
    }

    protected virtual (int, bool) CalculateActiveSpellDamage(int hitCount, float critChance, float critMultiplier, Spell.Stats spellData)
    {
        // Subtract damage for each subsequent hit (without going below 0)
        int damageSubtraction = 1 * (hitCount - 1);
        int damageOutput = Math.Max(0, _currentStats.Damage - damageSubtraction);
    
        bool isCriticalHit = false;

        // Calculate critical hit
        float roll = UnityEngine.Random.value; // Get a random value between 0.0 and 1.0
        if (roll <= critChance) // If the random value is less than or equal to the crit chance, a critical hit occurs
        {
            damageOutput = (int)(damageOutput * critMultiplier); // Multiply the damage by the critical hit multiplier
            isCriticalHit = true;
        }
        return (damageOutput, isCriticalHit);
    }
}
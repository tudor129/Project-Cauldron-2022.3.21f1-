/*
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PoisonAttackSpell : BaseActiveSpell
{
    //List<IAttackable> _attackablesInRadius = new List<IAttackable>();
    
    // public float speed = 15f;
    // public float hitOffset = 0f;
    // public bool UseFirePointRotation;
    // public Vector3 rotationOffset = new Vector3(0, 0, 0);
    // public GameObject hit;
    // public GameObject flash;
    // private Rigidbody rb;
    // public GameObject[] Detached;
    
    Light _light;
    // ParticleSystem _particleSystem;
    

    protected override void Awake()
    {
        base.Awake();
        _attackablesInRadius.Clear();
        _particleSystem = GetComponent<ParticleSystem>();
        Initialize(spellData, _player, PlayerData);


        if (_particleSystem.lights.enabled == false && _light != null)
        {
            _light = gameObject.AddComponent<Light>();
        }
        //_light = GetComponent<Light>();
        
    }
    protected override void Start()
    {
        _attackablesInRadius.Clear();
        if (_light != null)
            _light.intensity = 2f;
        
        _rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            
            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, spellData.BaseLifetime);

    }
   
    protected override void FixedUpdate ()
    {
        float frequency = 20f;
        float amplitude = 0.1f;
        float oscillationFactor = Mathf.Sin(Time.time * frequency);
        if (spellData.BaseSpeed != 0)
        {
            Vector3 snakeDirection = transform.forward + transform.right * oscillationFactor * amplitude;

            snakeDirection.Normalize();

            _rb.velocity = snakeDirection * spellData.BaseSpeed;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Spell"))
        {
            return;
        }
        
        Vector3 impactPoint = collision.contacts[0].point;
        float radius = spellData.BaseSpellRadius;
        
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
                _attackablesInRadius.Add(attackable);
            }
        }
       
        for (int i = _attackablesInRadius.Count - 1; i >= 0; i--)
        {
            IAttackable attackable = _attackablesInRadius[i];
            if (attackable != null && attackable.IsActive())
            {
                _hitCount++;
                (int damage, bool isCritical) = CalculateAttackDamage(_hitCount, PlayerData.GetCriticalChance(), PlayerData.GetCriticalMultiplier(), spellData);
                attackable.TakeDamage(damage, isCritical, spellData);
                
                if (isCritical)
                {
                    // Blood splatter
                }
            }
            else
            {
                _attackablesInRadius.RemoveAt(i);
            }
        }
       
        //Lock all axes movement and rotation
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        spellData.BaseSpeed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        //Spawn hit effect on collision
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
           
            int hitCount = _hitCount;
            hitCount = (int)Mathf.Clamp(hitCount, 0f, 3f);
            float spellRadiusOffset = spellData.BaseSpellRadius * hitCount * 0.5f;
            hitInstance.transform.localScale = new Vector3(spellRadiusOffset, spellRadiusOffset, spellRadiusOffset);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        //Removing trail from the projectile on collision enter or smooth removing. Detached elements must have "AutoDestroying script"
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }
        //Destroy projectile on collision
        Destroy(gameObject);
    }
    
    protected override (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, SpellData damageType)
    {
        // Subtract 10 damage for each subsequent hit (without going below 0)
        int damageSubtraction = 2 * (hitCount -1);
        // Calculate the actual attack damage
        spellData.DamageOutput = Math.Max(0, spellData.BaseDamage - damageSubtraction);
        
        bool isCriticalHit = false;
        
        // Calculate critical hit
        float roll = UnityEngine.Random.value; // Get a random value between 0.0 and 1.0
        if (roll <= critChance) // If the random value is less than or equal to the crit chance, a critical hit occurs
        {
            spellData.DamageOutput = (int)(spellData.DamageOutput * critMultiplier); // Multiply the damage by the critical hit multiplier
            isCriticalHit = true;
        }
        return (spellData.DamageOutput, isCriticalHit);
    }
}
*/

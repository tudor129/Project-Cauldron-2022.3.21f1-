/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WildfireProjectileSpell : BaseActiveSpell
{
    [FormerlySerializedAs("WildfireLeapSO")] public WildfireLeapData wildfireLeapData;
    
    List<Collider> _enemyColliders = new List<Collider>();
    
    int _leapCount;

    enum ProjectileState
    {
        SeekingTarget,
        KeepingPath
    }
    
    ProjectileState _currentState = ProjectileState.SeekingTarget;
    
    protected override void Awake()
    {
       _part = GetComponent<ParticleSystem>();
       _sphereCollider = GetComponent<SphereCollider>();
       _sphereCollider.isTrigger = true;
       _rigidbody = GetComponent<Rigidbody>();
       _rigidbody.isKinematic = true;
       _rigidbody.drag = 0.2f;
       
       Initialize(wildfireLeapData, _player, PlayerData);
    }
    
    
    protected override void Start()
    {
       base.Start();
       _enemyColliders.Clear();
       _hitCount = 0;
    }

    protected override void FixedUpdate()
    {
       
    }

    protected override void Update()
    {
        MoveForward();
        switch (_currentState)
        {
            case ProjectileState.KeepingPath:
                MoveForward();
                break;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        if (!_enemyColliders.Contains(other))
        {
            _enemyColliders.Add(other);
        }
        
        _hitCount++;
        

        if (_hitCount >= wildfireLeapData.LeapCount)
        {
            Destroy(gameObject);
        }
        
        Vector3 impactPoint = HandleImpactPoint(other);

        HandleProjectileImpactEffect(impactPoint);

        HandleProjectileTrailRemoval();
        
        DealDamage(other.GetComponent<IAttackable>());

        
        GameObject nearestEnemy = FindNearestEnemy(other);
            
        if (nearestEnemy != null)
        {
            Vector3 directionToEnemy = (nearestEnemy.transform.position - transform.position).normalized;
            
            // Adjust the direction of the projectile
            transform.forward = directionToEnemy;
            _currentState = ProjectileState.SeekingTarget;
        }
        else
        {
            // Continue in the same direction
            _currentState = ProjectileState.KeepingPath;
        }
        
        if (!spellData.IsPassThrough)
        {
            Destroy(gameObject);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position with a radius of searchRadius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
    
    void MoveForward()
    {
        Vector3 newPosition = transform.position + transform.forward * wildfireLeapData.BaseSpeed * Time.deltaTime;
    
        transform.position = newPosition;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        
    }
    
    GameObject FindNearestEnemy(Collider other)
    {
        float searchRadius = 10f; // Set the search radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        float shortestDistance = float.MaxValue;
        GameObject nearestEnemy = null;


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") && hitCollider.gameObject != other.gameObject)
            {
                var hit = hitCollider.GetComponent<Collider>();
                if (_enemyColliders.Contains(hit))
                {
                    continue;
                }
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < shortestDistance)
                {
                    
                    shortestDistance = distance;
                    nearestEnemy = hitCollider.gameObject;
                }
                
               
            }
        }
        // Debug.Log("returning nearest enemy: " + nearestEnemy.name);
        // Debug.Log("shortest distance is: " + shortestDistance);

        return nearestEnemy;
    }
   
    void DealDamage(IAttackable attackable)
    {
        if (attackable == null)
        {
            return;
        }
        (int damage, bool isCritical) = CalculateAttackDamage(_hitCount, PlayerData.GetCriticalChance(), PlayerData.GetCriticalMultiplier(), wildfireLeapData);
        attackable.TakeDamage(damage, isCritical, wildfireLeapData);
    }

    protected override (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, SpellData damageType)
    {
        // Random range between base damage and base damage + 15% of base damage
        wildfireLeapData.DamageOutput = Random.Range(wildfireLeapData.BaseDamage, wildfireLeapData.BaseDamage + (int)(wildfireLeapData.BaseDamage * 0.15f));
        
        
        bool isCriticalHit = false;
        
        // Calculate critical hit
        float roll = UnityEngine.Random.value; // Get a random value between 0.0 and 1.0
        if (roll <= critChance) // If the random value is less than or equal to the crit chance, a critical hit occurs
        {
            wildfireLeapData.DamageOutput = (int)(wildfireLeapData.DamageOutput * critMultiplier); // Multiply the damage by the critical hit multiplier
            isCriticalHit = true;
        }
        return (wildfireLeapData.DamageOutput, isCriticalHit);
    }
}
*/

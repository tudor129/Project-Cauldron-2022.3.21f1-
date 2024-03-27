/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ExplosionBombSpell : Spell
{
    List<IAttackable> _attackablesInRadius = new List<IAttackable>();
    
    protected override void Awake()
    {
        base.Awake(); 
        _attackablesInRadius.Clear();
        Initialize(spellData, _player, PlayerData);
        Destroy(gameObject, spellData.BaseLifetime);
        _part = GetComponent<ParticleSystem>();

        _spawnPosition = GameManager.Instance.GetMouseWorldPosition();
    }
    protected override void Start()
    {
        transform.position = _spawnPosition + new Vector3(0, 0.5f, 0);
    }


    protected override (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, SpellData damageType)
    {
        // Random range between base damage and base damage + 25% of base damage
        spellData.DamageOutput = Random.Range(spellData.BaseDamage, spellData.BaseDamage + (int)(spellData.BaseDamage * 0.25f));
      
        return (spellData.DamageOutput, false);
    }
    
    /*protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Player"))
        {
            return;
        }
        IAttackable attackable = other.GetComponent<IAttackable>();
        if (attackable != null)
        {
            (int damage, bool isCritical) = CalculateAttackDamage(_hitCount, _playerStat.GetCriticalChance(), _playerStat.GetCriticalMultiplier());
            attackable.TakeDamage(damage, false);
        }
    }#1#

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _part.GetCollisionEvents(other, _collisionEvents); 
        for (int i = 0; i < numCollisionEvents; i++)
        {
            Collider[] collidersInSphere = Physics.OverlapSphere(_collisionEvents[i].intersection, spellData.BaseSpellRadius);
            
            foreach (var collider in collidersInSphere)
            {
                if (collider.CompareTag("Player"))
                {
                    continue;
                }
                
                collider.TryGetComponent(out IAttackable attackable);

                if (attackable != null && !_attackablesInRadius.Contains(attackable))
                {
                    _attackablesInRadius.Add(attackable);
                    (int damage, bool isCritical) = CalculateAttackDamage(_hitCount, PlayerData.GetCriticalChance(), PlayerData.GetCriticalMultiplier(), spellData);
                    attackable.TakeDamage(damage, false, spellData);
                    
                }
            }
        }
    }

    void OnParticleTrigger()
    {
        // particles = 1 when the particle is inside the trigger
        // particles = 0 when the particle is outside the trigger
        //int numEnter = _part.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enter);
        //int numExit = _part.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, _exit);
        //Debug.Log("Enter: " + numEnter + ", Exit: " + numExit);
    }


}
*/

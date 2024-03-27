/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlameLogic : BaseActiveSpell
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (_spellInstance.Spell.BaseSpeed > 0)
        {
            transform.Translate(Vector3.forward * spellData.BaseSpeed * Time.deltaTime, Space.Self);
        }
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Spell") || other.CompareTag("Loot"))
        {
            return;
        }
        Vector3 impactPoint = other.transform.position;
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
    }

}
*/

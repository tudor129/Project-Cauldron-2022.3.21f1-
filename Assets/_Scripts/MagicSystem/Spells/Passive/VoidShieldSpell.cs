/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class VoidShieldSpell : Spell
{
    List<IAttackable> _attackablesInRange = new List<IAttackable>();

  
    Vector3 _spawnOffset = new Vector3(0, 0.5f, 0);
    
    protected override void Awake()
    {
        _player = GameManager.Instance.player;

        Initialize(spellData, _player, PlayerData);
        Destroy(gameObject, spellData.BaseLifetime);
        StartCoroutine(DamageTick());
    }

    protected override void Start()
    {
        _spawnPosition = _player.transform.position + new Vector3(0, 0f, 0);
        _spawnRotation = transform.rotation;
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected override void Update()
    {
        transform.position = _player.transform.position + _spawnOffset;
        transform.rotation = _spawnRotation;
    }

    IEnumerator DamageTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = _attackablesInRange.Count - 1; i >= 0; i--)
            {
                var attackable = _attackablesInRange[i];
                if (attackable != null && attackable.IsActive())
                {
                    attackable.TakeDamage(spellData.BaseDamage, false, spellData);
                }
                else
                {
                    _attackablesInRange.RemoveAt(i);
                }
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        IAttackable attackable = other.GetComponent<IAttackable>();
        if (attackable != null && !_attackablesInRange.Contains(attackable))
        {
            _attackablesInRange.Add(attackable);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        IAttackable attackable = other.GetComponent<IAttackable>();
        if (attackable != null)
        {
            _attackablesInRange.Remove(attackable);
        }
    }

    protected override (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, SpellData damageType)
    {
        return (spellData.BaseDamage, false);
    }

   
}
*/

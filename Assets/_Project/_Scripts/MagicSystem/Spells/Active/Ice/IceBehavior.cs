using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBehavior : BaseSpellBehavior
{
    SphereCollider _sphereCollider;
    
    protected override void Awake()
    {
        _part = GetComponent<ParticleSystem>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    protected override void Start()
    {
        CoroutineManager.Instance.StartManagedCoroutine(TurnOffColliderAfterDelay(_part.main.duration));
    }

    void OnEnable()
    {
        _sphereCollider.enabled = true;
    }

    void OnDisable()
    {
        _sphereCollider.enabled = false;
    }
    
    public void Initialize(Spell.Stats stats)
    {
        _currentStats = stats;
    }
    
    IEnumerator TurnOffColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _sphereCollider.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            enemy._currentState = Enemy.EnemyState.Slowed;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            enemy._currentState = Enemy.EnemyState.Walking;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpellImpactBehavior : BaseSpellBehavior
{
    protected override void Awake()
    {
       
    }

    protected override void Start()
    {
        base.Start();
        //Destroy(gameObject, _currentStats.Lifetime);
        _currentStats = spell.GetStats();
        //StartCoroutine(DespawnAfterDelay(_currentStats.ImpactLifetime));
    }

    void OnEnable()
    {
        //StartCoroutine(DespawnAfterDelay(_currentStats.ImpactLifetime));
    }

    IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // wait for 'delay' seconds
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject); // then call your function to return the game object to the pool
    }

    
}

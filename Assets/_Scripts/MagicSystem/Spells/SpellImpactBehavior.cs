using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellImpactBehavior : SpellBehavior
{

    protected override void Awake()
    {
       
    }

    protected override void Start()
    {
        base.Start();
        //Destroy(gameObject, _currentStats.Lifetime);
        ObjectPoolManager.Instance.ReturnEnemyObjectToPool(gameObject);
    }

    
}

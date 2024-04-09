using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehavior : BaseSpellBehavior
{
    
    SphereCollider _sphereCollider;

    protected override void Awake()
    {
        base.Awake();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    protected override void Start()
    {
        
    }
    
    public void Initialize(RainBehavior caller, Spell.Stats stats)
    {
        _currentStats = stats;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().ApplyDoTEffect(_currentStats);
            other.transform.Find("Spell_Light_6_LWRP").gameObject.SetActive(true);
        }
    }


}

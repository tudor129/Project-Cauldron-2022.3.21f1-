using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class Attack : MonoBehaviour
{
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] GameObject _attackEffectPrefab;
    [SerializeField] Transform _objectHolder;
    
    float _lastAttackTime = -1f;
    float _attackCooldown = 0.1f;
    int _attackDamage;


    void Awake()
    {
    }

    // int CalculateAttackDamage()
    // {
    //     Random rand = new Random();
    //     _attackDamage = rand.Next(10, 51); // Note: the upper bound is exclusive, so you need to add 1 more than your desired maximum value.
    //     return _attackDamage;
    // }
    
    public void PerformAttack()
    {
        var particleSystem = ObjectPoolManager.Instance.SpawnObject(_attackEffectPrefab, _objectHolder.transform.position, transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);
        
    }
}

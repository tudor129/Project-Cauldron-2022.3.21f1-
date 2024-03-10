using DamageNumbersPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    public static DamageNumberSpawner Instance { get; private set; }

    [SerializeField] DamageNumberLogic _damageNumberPrefab;
    [SerializeField] DamageNumberLogic _criticalDamageNumberPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (_damageNumberPrefab != null)
        {
            _damageNumberPrefab.GetComponent<DamageNumberLogic>();
        }
        else
        {
            Debug.LogError("No prefab assigned to _damageNumberPrefab in DamageNumberSpawner");
        }
        if (_criticalDamageNumberPrefab != null)
        {
            _criticalDamageNumberPrefab.GetComponent<DamageNumberLogic>();
        }
        else
        {
            Debug.LogError("No prefab assigned to _criticalDamageNumberPrefab in DamageNumberSpawner");
        }
    }

    public void SpawnDamageNumber(Vector3 position, int damage)
    {
        _damageNumberPrefab.Spawn(position, damage);
    }
    
    public void SpawnCriticalDamageNumber(Vector3 position, int damage)
    {
        _criticalDamageNumberPrefab.Spawn(position, damage);
    }

}

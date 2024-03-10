using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealthBarUI : HealthBarUI
{
    [SerializeField] GameObject _healthBarVisual;
    
    EnemyHealth _associatedEnemy;


    void Start()
    {
        _healthBarVisual.SetActive(false);
    }

    void OnEnable()
    {
        EventManager<HealthEventPayload>.RegisterEvent(EventKey.UPDATE_ENEMY_HEALTH, HandleEnemyHealthChanged);
    }
    
    void OnDisable()
    {
        EventManager<HealthEventPayload>.UnregisterEvent(EventKey.UPDATE_ENEMY_HEALTH, HandleEnemyHealthChanged);
    }
    
    void HandleEnemyHealthChanged(HealthEventPayload payload)
    {
        if (payload.EnemyHealthInstance == _associatedEnemy)
        {
            _healthBarVisual.SetActive(true);
            _slider.value = payload.CurrentHealth;
            if (payload.CurrentHealth <= 0)
            {
                _slider.value = 0;
            }
        }
    }

    public void Initialize(EnemyHealth enemy)
    {
        _associatedEnemy = enemy;
    }
}

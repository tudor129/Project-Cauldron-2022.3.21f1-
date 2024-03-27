using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] protected Slider _slider;

    void OnEnable()
    {
        EventManager<int>.RegisterEvent(EventKey.UPDATE_HEALTH, SetCurrentHealth);
    }
    
    void OnDisable()
    {
        EventManager<int>.UnregisterEvent(EventKey.UPDATE_HEALTH, SetCurrentHealth);
    }

    void SetMaxHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }
    
    void SetCurrentHealth(int currentHealth)
    {
        _slider.value = currentHealth;
    }
   
  
}

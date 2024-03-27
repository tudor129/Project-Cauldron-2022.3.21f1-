using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ManaBarUI : MonoBehaviour
{
    [SerializeField] Slider _slider;

    void Awake()
    {
        if (_slider == null)
        {
            _slider = GetComponent<Slider>();
        }
    }
    
    void OnEnable()
    {
        EventManager<float>.RegisterEvent(EventKey.UPDATE_MANA, SetCurrentMana);
        EventManager<float>.RegisterEvent(EventKey.UPDATE_MAX_MANA, SetMaxMana);
    }
    void OnDisable()
    {
        EventManager<float>.UnregisterEvent(EventKey.UPDATE_MANA, SetCurrentMana);
        EventManager<float>.UnregisterEvent(EventKey.UPDATE_MAX_MANA, SetMaxMana);
    }

    void SetMaxMana(float maxMana)
    {
        _slider.maxValue = maxMana;
        _slider.value = maxMana;
    }
    
    void SetCurrentMana(float currentMana)
    {
        _slider.value = currentMana;
    }
}

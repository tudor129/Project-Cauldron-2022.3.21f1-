using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager<TEventArgs>
{
    static Dictionary<string, Action<TEventArgs>> _eventDictionary = new Dictionary<string, Action<TEventArgs>>();

    public static void RegisterEvent(string eventType, Action<TEventArgs> eventHandler)
    {
        if (!_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] = eventHandler;
        }
        else
        {
            _eventDictionary[eventType] += eventHandler;
        }
    }
    
    public static void UnregisterEvent(string eventType, Action<TEventArgs> eventHandler)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] -= eventHandler;
        }
    }
    
    public static void TriggerEvent(string eventType, TEventArgs eventArgs)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType]?.Invoke(eventArgs);
        }
    }
    
    public static void ClearAllEvents()
    {
        _eventDictionary.Clear();
    }
}

public static class EventKey
{
    public static string UPDATE_HEALTH = "OnPlayerHealthChanged";
    public static string UPDATE_MAX_HEALTH = "OnPlayerMaxHealthChanged";
    public static string UPDATE_MANA = "OnPlayerManaChanged";
    public static string UPDATE_MAX_MANA = "OnPlayerMaxManaChanged";
    public static string UPDATE_EXPERIENCE = "OnAddExperience";
    public static string UPDATE_MAX_EXPERIENCE = "OnPlayerMaxExperienceChanged";
    public static string UPDATE_EXPERIENCE_AFTER_LEVEL_UP = "OnPlayerExperienceAfterLevelUpChanged";
    public static string FINISH_XP_BAR_TWEEN = "OnFinishXpBarTween";
    public static string UPDATE_LEVEL = "OnPlayerLevelUp";
    public static string SPELL_CAST = "OnSpellCast";
    public static string ABILITY_CAST = "OnAbilityCast";
    
    public static string UPDATE_ENEMY_HEALTH = "OnEnemyHealthChanged";
    
    public static string ANIMATION_EVENT = "OnAnimationEvent";
    
}

[System.Serializable]
public class HealthEventPayload
{
    public int CurrentHealth;
    public EnemyHealth EnemyHealthInstance;
}

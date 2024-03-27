using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CooldownManagerEventArgs
{
    public SpellData SpellData;
    public AbilityData AbilityData;
}

public class CooldownManager : MonoBehaviour
{
    public static CooldownManager Instance { get; private set; }
   
    [SerializeField, ShowInInspector]
    Dictionary<string, float> _cooldownTimers = new Dictionary<string, float>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Updates all cooldown timers and removes any timers that have finished.
    /// </summary>
    /// <remarks>
    /// This method decrements the time remaining on each cooldown timer by the time passed
    /// since the last frame. It then checks if any timers have finished (reached zero or less),
    /// and removes them from the cooldown timer dictionary.
    /// </remarks>
    void Update()
    {
        List<string> keysToRemove = new List<string>();

        // Create a copy of the keys to iterate over
        List<string> keys = new List<string>(_cooldownTimers.Keys);

        // Update all cooldown timers
        foreach (var key in keys)
        {
            _cooldownTimers[key] -= Time.deltaTime;

            // Check if cooldown is finished
            if (_cooldownTimers[key] <= 0)
            {
                keysToRemove.Add(key);
            }
        }

        // Remove cooldowns that have finished
        foreach (var key in keysToRemove)
        {
            _cooldownTimers.Remove(key);
        }
    }

    public void StartCooldown(string abilityName, float cooldownDuration)
    {
        if (!IsOnCooldown(abilityName))
        {
            _cooldownTimers.Add(abilityName, cooldownDuration);
        }
    }

    public bool IsOnCooldown(string abilityName)
    {
        return _cooldownTimers.ContainsKey(abilityName);
    }
}


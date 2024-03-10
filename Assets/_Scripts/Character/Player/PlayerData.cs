using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Stats/PlayerStatSO", order = 1)]
public class PlayerData : SerializedScriptableObject
{ 
    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }
    
    [SerializeField]
    SpellData startingSpell;
    public SpellData StartingSpell { get => startingSpell; private set => startingSpell = value; }
    
    [System.Serializable]
    public struct Stats
    {
        public float maxHealth;
        public float recovery;
        public float moveSpeed;
        public float magnetSpeed;
        public float criticalChance;
        public float criticalMultiplier;
        
        public Stats(float maxHealth = 1000, 
                     float recovery = 0, 
                     float moveSpeed = 1f, 
                     float magnetSpeed = 30f, 
                     float criticalChance = 0.1f, 
                     float criticalMultiplier = 1.5f)
        {
            this.maxHealth = maxHealth;
            this.recovery = recovery;
            this.moveSpeed = moveSpeed;
            this.magnetSpeed = magnetSpeed;
            this.criticalChance = criticalChance;
            this.criticalMultiplier = criticalMultiplier;
        }
      
        
        public static Stats operator +(Stats s1, Stats s2)
        {
            s1.maxHealth += s2.maxHealth;
            s1.recovery += s2.recovery;
            s1.moveSpeed += s2.moveSpeed;
            s1.magnetSpeed += s2.magnetSpeed;
            return s1;
        }
    }
    
    protected Stats _currentStats;

    public Stats GetStats()
    {
        return _currentStats;
    }
    

    public Dictionary<Stat, float> _stats = new Dictionary<Stat, float>();
    
    [Header("Critical Hit Variables")]
    [SerializeField] float _baseCritChance = 0.1f; // 10% base crit chance
    [SerializeField] float _critChanceIncreasePerLevel = 0.1f; // Increase crit chance by x% per level
    [SerializeField] float _baseCritMultiplier = 1.5f; // Base crit multiplier
    [SerializeField] float _critMultiplierIncreasePerLevel = 0.5f; // Increase crit multiplier by x per level
    
    [FormerlySerializedAs("_baseMaxhealth")]
    [Header("Health Variables")]
    [SerializeField] int _baseMaxHealth = 100;
    [Header("Magic Variables")]
 
    
    PlayerStats _playerStats;
    
    public float GetStat(Stat stat)
    {
        if (_stats.TryGetValue(stat, out float value))
        {
            return value;
        }
        else
        {
            Debug.LogError("Stat " + stat + " not found in dictionary!");
            return 0;
        }
    }
    
    public void SetPlayerProgression(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }
    
    public PlayerStats GetPlayerProgression()
    {
        return _playerStats;
    }
    
    
    public int SetMaxHealth(int maxHealth)
    {
        return _baseMaxHealth = maxHealth;
    }
    
    public float GetCriticalChance()
    {
        //return _baseCritChance + _critChanceIncreasePerLevel * (_playerProgression.GetCurrentLevel() - 1);
        return GetStat(Stat.BaseCriticalChance) + GetStat(Stat.CriticalChancePerLevel) * (_playerStats.GetCurrentLevel() - 1);
    }

    public float GetCriticalMultiplier()
    {
        return _baseCritMultiplier + _critMultiplierIncreasePerLevel * (_playerStats.GetCurrentLevel() - 1);
    }
    
    public int GetMaxHealth()
    {
        return _baseMaxHealth + 10 * (_playerStats.GetCurrentLevel() - 1);
    }
  
    
}

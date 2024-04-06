using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SpellInstance
{
    public Spell Instance;
    public SpellData Spell;
     
    Dictionary<Type, int> _upgradeLevels = new Dictionary<Type, int>();
    
    [FormerlySerializedAs("BaseDamage")] public int Damage;
    [FormerlySerializedAs("BaseSpeed")] public float Speed;
    [FormerlySerializedAs("BaseLifetime")] public float Lifetime;
    [FormerlySerializedAs("BaseSpellRadius")] public float SpellRadius;
    [FormerlySerializedAs("BaseCooldown")] public float Cooldown;
    [FormerlySerializedAs("BaseDamageOverTimeDamage")] public int DamageOverTimeDamage;
    [FormerlySerializedAs("BaseDamageOverTimeDuration")] public float DamageOverTimeDuration;
    [FormerlySerializedAs("BaseDamageOverTimeInterval")] public float DamageOverTimeInterval;
    [FormerlySerializedAs("BaseDamageOverTimeInitialDelay")] public float DamageOverTimeInitialDelay;
    [FormerlySerializedAs("BaseStaggerDuration")] public float StaggerDuration;
    
    
    public SpellInstance(SpellData spellData)
    {
        Spell = spellData;
        Damage = spellData.BaseStats.Damage;
        Speed = spellData.BaseStats.Speed;
        Lifetime = spellData.BaseStats.Lifetime;
        SpellRadius = spellData.BaseStats.SpellRadius;
        Cooldown = spellData.BaseStats.Cooldown;  
        DamageOverTimeDamage = spellData.BaseStats.DamageOverTimeDamage;
        DamageOverTimeDuration = spellData.BaseStats.DamageOverTimeDuration;
        DamageOverTimeInterval = spellData.BaseStats.DamageOverTimeInterval;
        DamageOverTimeInitialDelay = spellData.BaseStats.DamageOverTimeInitialDelay;
        StaggerDuration = spellData.BaseStats.StaggerDuration;
    
        
        
        Debug.Log($"Spell {Spell.name} created with {Damage} damage and {Cooldown} cooldown.");
    }

   

    
    public void ApplyUpgradeTest()
    {
        Instance.LevelUp();
    }
    
    public virtual void ApplyUpgrade(SpellStatUpgradeData statUpgrade)
    {
        
        
    }
    public virtual SpellInstance ApplyUpgradeUsingReflection(SpellInstance spellInstance, SpellStatUpgradeData statUpgrade, float modifier)
    {
        // THE DECORATOR TYPE NAME MUST MATCH THE NAME OF THE DECORATOR CLASS
        // THIS MUST BE WRITTEN MANUALLY IN THE ITEM NAME FROM STAT UPGRADE SCRIPTABLE OBJECT
        // E.G. DAMAGEUPGRADEDECORATOR, DAMAGEOVERTIMEUPGRADEDECORATOR
        
        var decoratorTypeName = statUpgrade.GetDecoratorTypeName(); 
        Type decoratorType = Type.GetType(decoratorTypeName);
        if (decoratorType != null)
        {
             return (SpellInstance)Activator.CreateInstance(decoratorType, new object[] { spellInstance, statUpgrade, modifier });
            
            // var upgradedInstance = (SpellInstance)Activator.CreateInstance(decoratorType, new object[] { spellInstance, statUpgrade, modifier });
            //
            // int currentLevel = GetUpgradeLevel(decoratorType);
            //
            // SetUpgradeLevel(decoratorType, currentLevel + 1);
            //
            // return upgradedInstance;
        }
        else
        {
            Debug.LogWarning($"Decorator type {decoratorTypeName} not found.");
            return spellInstance; // Return the original instance if no matching decorator is found
        }
    }
    
    public int GetUpgradeLevel(Type upgradeType)
    {
        if (_upgradeLevels.TryGetValue(upgradeType, out int level))
        {
            return level;
        }
        return 0;
    }

    public void SetUpgradeLevel(Type upgradeType, int level)
    {
        _upgradeLevels[upgradeType] = level;
    }
    
    public void IncrementUpgradeLevel(Type upgradeType)
    {
        // If the upgrade type is already in the dictionary, increment its level
        if (_upgradeLevels.ContainsKey(upgradeType))
        {
            _upgradeLevels[upgradeType]++;
        }
        else
        {
            // If the upgrade type is new, add it to the dictionary with a level of 1
            _upgradeLevels[upgradeType] = 1;
        }
    }
    
  
   
}

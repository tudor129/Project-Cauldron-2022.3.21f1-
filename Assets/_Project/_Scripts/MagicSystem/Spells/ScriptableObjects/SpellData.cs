using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell")]
public class SpellData : ItemData
{
    [HideInInspector] public string Behaviour;
    public Spell.Stats BaseStats;
    public Spell.Stats[] LinearGrowth;
    public Spell.Stats[] RandomGrowth;
    
    // Gives us the stat growth / description of the next level.
    public Spell.Stats GetLevelData(int level)
    {
        // Pick the stats from the next level.
        if (level - 2 < LinearGrowth.Length)
            return LinearGrowth[level - 2];

        // Otherwise, pick one of the stats from the random growth array.
        if (RandomGrowth.Length > 0)
            return RandomGrowth[Random.Range(0, RandomGrowth.Length)];

        // Return an empty value and a warning.
        Debug.LogWarning(string.Format("Weapon doesn't have its level up stats configured for Level {0}!",level));
        return new Spell.Stats();
    }

  
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Potion : Item
{
     public PotionData potionData;

    [System.Serializable]
    public struct Modifier
    {
        public string name;
        public string description;
        public PlayerData.Stats boosts;
    }
    
    
    // Levels up the weapon by 1, and calculates the corresponding stats.
    /*public override bool DoLevelUp()
    {
        base.DoLevelUp();

        // Prevent level up if we are already at max level.
        if (!CanLevelUp())
        {
            Debug.LogWarning(string.Format("Cannot level up {0} to Level {1}, max level of {2} already reached.", name, currentLevel, data.maxLevel));
            return false;
        }

        // Otherwise, add stats of the next level to our weapon.
        currentBoosts += data.GetLevelData(++currentLevel).boosts;
        return true;
    }*/
}

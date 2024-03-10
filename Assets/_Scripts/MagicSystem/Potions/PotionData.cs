using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionData : ItemData
{
    public Potion.Modifier baseStats;
    public Potion.Modifier[] growth;

    public Potion.Modifier GetLevelData(int level)
    {
        // Pick the stats from the next level.
        if (level - 2 < growth.Length)
            return growth[level - 2];

        // Return an empty value and a warning.
        Debug.LogWarning(string.Format("Passive doesn't have its level up stats configured for Level {0}!", level));
        return new Potion.Modifier();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomLoot : MonoBehaviour
{
    public int[] LootTable =
    {
        60, // gems
        30, // coins
        10 // chest

    };

    void Start()
    {
        int randomValue = Random.Range(0, LootTable[LootTable.Length - 1]);

        for (int i = 0; i < LootTable.Length; i++)
        {
            if (randomValue < LootTable[i])
            {
                // drop item at index i
                break;
            }
        }
    }
}

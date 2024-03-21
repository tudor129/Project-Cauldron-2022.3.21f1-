using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one LootManager instance!" + gameObject.name + " is being destroyed!");
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void DropLoot(Transform enemyTransform, EnemySO enemyData)
    {
        float cumulativeChance = 0;
        int randomValue = Random.Range(0, 100); // Assuming the sum of _dropChance of all items in LootTable is 100

        foreach (LootSO loot in enemyData.LootTable)
        {
            cumulativeChance += loot._dropChance;

            if (randomValue < cumulativeChance)
            {
                Quaternion rotation = Quaternion.Euler(loot._rotationOffset);
                // GameObject droppedLoot = Instantiate(
                //     loot._lootPrefab, 
                //     enemyTransform.position + new Vector3(0, loot._spawnHeightOffset, 0), 
                //     rotation);
                
                GameObject droppedLoot = ObjectPoolManager.Instance.SpawnObject(
                    loot._lootPrefab, 
                    enemyTransform.position + new Vector3(0, loot._spawnHeightOffset, 0), 
                    rotation, 
                    ObjectPoolManager.PoolType.GameObject);

                
                if (droppedLoot.name.Contains("Chest_Closed"))  
                {
                    droppedLoot.transform.rotation = Quaternion.Euler(-90, 0, 0);  
                }
                else if(droppedLoot.name.Contains("Gem"))
                {
                    droppedLoot.transform.rotation = Quaternion.Euler(0, 0, -90); 
                } 
                else if (droppedLoot.name.Contains("Coin"))
                {
                    droppedLoot.transform.rotation = Quaternion.Euler(0, 0, -90);
                }

                break;
            }
        }
        
    }
}

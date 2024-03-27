using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Enemy/EnemySO", order = 1)]
public class EnemySO : ScriptableObject
{
    public string EnemyName;
    public GameObject EnemyPrefab;
    public List<LootSO> LootTable = new List<LootSO>(); // List of possible loot drops
    public float MoveSpeed = 2f;
    public float AttackRange = 2f;
    public float AttackCooldown = 1f;
    public int AttackDamage = 5;
    
    public string GetEnemyName()
    {
        return EnemyName;
    }
}

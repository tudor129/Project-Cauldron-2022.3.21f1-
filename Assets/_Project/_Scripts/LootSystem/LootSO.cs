using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootSO", menuName = "Loot/LootSO", order = 1)]
public class LootSO : ScriptableObject
{
    public Sprite _lootIcon;
    public GameObject _lootPrefab;
    public string _lootName;
    public float _dropChance;
    public Vector3 _rotationOffset = Vector3.zero;
    public float _spawnHeightOffset = 0;

    
    
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Tooltip("If this is a stat upgrade, this MUST MATCH the name of decorator class name")]
    public string ItemName;
    public int MaxLevel;
    [Required] public Sprite icon;
    
    
    
    // Targeting Strategy
    // Factory Pattern for creating a new instance of the item
    // Other Item details, prefabs, etc.
}

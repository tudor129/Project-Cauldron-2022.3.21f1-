using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Spell Upgrade", menuName = "Spell/Stat Upgrade")]
public class SpellStatUpgradeData : ItemData
{
    public bool IsStatUpgrade;
    
    [Tooltip("The value to modify the stat by.")]
    [FormerlySerializedAs("DamageModifier")] public float Modifier;
    public float ModifierLevel2;
    public float ModifierLevel3;
    public float ModifierLevel4;
    public float ModifierLevel5;
    
    public string UpgradeTypeIdentifier;
    
    public string GetDecoratorTypeName()
    {
        return ItemName;
    }
    
    public float GetModifierByLevel(int level)
    {
        switch (level)
        {
            case 1: return Modifier;
            case 2: return ModifierLevel2;
            case 3: return ModifierLevel3;
            case 4: return ModifierLevel4;
            case 5: return ModifierLevel5;
            default: 
                Debug.LogWarning($"Modifier for level {level} is not defined. Defaulting to base Modifier.");
                return Modifier;
        }
    }
   
}

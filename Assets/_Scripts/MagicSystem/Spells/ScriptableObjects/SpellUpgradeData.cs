using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Upgrade", menuName = "Spell/Spell Upgrade")]
public class SpellUpgradeData : ItemData
{
     public bool IsSpellUpgrade;
     
     public ScriptableObject SpellUpgrade;
}

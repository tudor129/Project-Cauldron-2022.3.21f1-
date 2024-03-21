using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
   public int CurrentLevel = 1;
   public int MaxLevel = 1;
   
   protected SpellManager _spellManager;
   protected PlayerStats _playerStats;
   protected Player _player;
   
   public virtual void Initialise(ItemData data)
   {
      MaxLevel = data.MaxLevel;

      // We have to find a better way to reference the player SpellManager
      // in the future, as this is inefficient.
      _spellManager = FindObjectOfType<SpellManager>();
      _playerStats = FindObjectOfType<PlayerStats>();
      _player = FindObjectOfType<Player>();
      
   }
   
   // What effects you receive on equipping an item.
   public virtual void OnEquip() { }

   // What effects are removed on unequipping an item.
   public virtual void OnUnequip() { }
   
}

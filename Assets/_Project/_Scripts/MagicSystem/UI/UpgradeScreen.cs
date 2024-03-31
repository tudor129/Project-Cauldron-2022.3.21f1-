using System;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class UpgradeScreen : HotBar
{
       public List<SpellData> _availableSpells = new List<SpellData>();
       public List<UISlot> _spellSlots = new List<UISlot>();
        
       [SerializeField] SpellManager _spellManager;
        
       protected override void Start()
       {
            base.Start();
            for (int i = 0; i < _slots.Count; i++)
            { 
                _slots[i].SetupUpgradeScreen(this);
            }
       }
       void OnEnable()
       {
           _spellManager.RemoveAndApplyUpgrades();
       }
       void OnDisable()
       {
           GameManager.Instance.ResumeGame();
       }
       

    public void OnClickUpgradeButton(UISlot clickedSlot)
    {
        // Get the index of the slot from the _slots list.
        int slotIndex = GetItemIndex(clickedSlot);
        // Perform a safety check.
        if (slotIndex < 0 || slotIndex >= _items.Count)
        {
            Debug.LogWarning("Clicked slot is not within the range of assigned items.");
            return;
        }
        // Get the associated Item
        ItemData itemData = _items[slotIndex];
        //_spellManager.LevelUpSpell(slotIndex, item);
        
    }
       
     public void OnSlotClick(UISlot clickedSlot)
     {
         // Get the index of the slot from the _slots list.
         int slotIndex = GetItemIndex(clickedSlot);
         // Perform a safety check.
         if (slotIndex < 0 || slotIndex >= _items.Count)
         {
             Debug.LogWarning("Clicked slot is not within the range of assigned items.");
             return;
         }
         // Get the associated Item
         ItemData itemData = _items[slotIndex];
         // Now you have the clicked item, and you can proceed with your upgrade logic.
         //UpgradeSpell(itemData);
         //gameObject.SetActive(false);
     }
}

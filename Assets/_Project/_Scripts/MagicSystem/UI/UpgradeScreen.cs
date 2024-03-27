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
       void UpgradeSpell(ItemData spellUpgrade)
       {
           /*if (spellUpgrade is SpellUpgradeSO spellUpgradeSO)
           {
               if (spellUpgradeSO.IsSpellUpgrade)
               {
                   // Swap the spell out for the upgraded spell
                   SpellSO spellSO = spellUpgradeSO.SpellUpgrade as SpellSO;
                   
                   _spellManager.AddNewActiveSpellInstance(spellSO);
               }
               else
               {
                   Debug.LogWarning("Item is not a Spell Upgrade");
               }
           }
           else if (spellUpgrade is SpellStatUpgradeSO spellStatUpgradeSO)
           {
               if (spellStatUpgradeSO.IsStatUpgrade)
               {
                   // Apply the stat upgrade to the spell
                   _spellManager.ApplyStatUpgrade(spellStatUpgradeSO);
                   Debug.Log("Applying Stat Upgrade " + spellStatUpgradeSO.name);
               }
               else
               {
                   Debug.LogWarning("Item is not a Stat Upgrade");
               }
           }
           else
           {
               Debug.LogWarning("Item is not a Spell Upgrade or a Stat Upgrade");
           }*/
             
       }
       
        /*void ApplyUpgradeOptions(UISlot clickedSlot)
    {
        // Make a duplicate of the available weapon / passive upgrade lists
        // so we can iterate through them in the function.
        List<SpellSO> availableSpellUpgrades = new List<SpellSO>(_spellManager._availableSpells);
      

        // Iterate through each slot in the upgrade UI.
        foreach (UISlot upgradeOption in _spellSlots)
        {
            // If there are no more avaiable upgrades, then we abort.
            if (availableSpellUpgrades.Count == 0)
                return;

            // Pick a weapon upgrade, then remove it so that we don't get it twice.
            SpellSO chosenSpellUpgrade = availableSpellUpgrades[UnityEngine.Random.Range(0, availableSpellUpgrades.Count)];
            availableSpellUpgrades.Remove(chosenSpellUpgrade);

            // Ensure that the selected weapon data is valid.
            if (chosenSpellUpgrade != null)
            {
                // Turns on the UI slot.
                //EnableUpgradeUI(upgradeOption);

                // Loops through all our existing weapons. If we find a match, we will
                // hook an event listener to the button that will level up the weapon
                // when this upgrade option is clicked.
                bool isLevelUp = false;
                for (int i = 0; i < _spellManager._acquiredSpells.Count; i++)
                {
                    SpellInstance w = _spellManager._acquiredSpells[i] as SpellInstance;
                    if (w != null && w.Instance.SpellSO == chosenSpellUpgrade)
                    {
                        // If the weapon is already at the max level, do not allow upgrade.
                        if (chosenSpellUpgrade.MaxLevel <= w.Instance.CurrentLevel)
                        {
                            //DisableUpgradeUI(upgradeOption);
                            isLevelUp = true;
                            break;
                        }

                        // Set the Event Listener, item and level description to be that of the next level
                   
                        upgradeOption._button.onClick.AddListener(() => _spellManager.LevelUpSpell(i, i)); //Apply button functionality
                        Spell.Stats nextLevel = chosenSpellUpgrade.GetLevelData(w.Instance.CurrentLevel + 1);
                    
                        isLevelUp = true;
                        break;
                    }
                }

                // If the code gets here, it means that we will be adding a new weapon, instead of
                // upgrading an existing weapon.
                if (!isLevelUp)
                {
                    //upgradeOption._button.onClick.AddListener(() => Add(chosenSpellUpgrade)); //Apply button functionality
                }
            }

        }
    }*/

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
         UpgradeSpell(itemData);
         //gameObject.SetActive(false);
     }
}

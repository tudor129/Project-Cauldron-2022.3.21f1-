using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameDevTV.Inventories
{
    [CreateAssetMenu(fileName = "CraftedItem", menuName = "Crafting/CraftedItem", order = 2)]
    public class RecipeSO : ItemSO
    {
        [System.Serializable]
        public struct RequiredMaterial
        {
            public Ingredient requiredIngredient;
            public int quantity;
        }

        [FormerlySerializedAs("requiredMaterials")]
        [Tooltip("List of materials required to craft this item.")]
        [SerializeField] List<RequiredMaterial> _requiredMaterials;
        [FormerlySerializedAs("_craftingTime")]
        [Tooltip("Crafting time in seconds.")]
        [SerializeField] float _craftingDuration;
        [Tooltip("The color of the Potion.")]
        [SerializeField] Color _potionColor;
        [Tooltip("The time of day this potion can be crafted.")]
        [SerializeField] CraftTimeOfDay _craftTimeOfDay = CraftTimeOfDay.Any;
        
        [SerializeField] int _xpReward;
        
        public enum CraftTimeOfDay
        {
            Day,
            Night,
            Any,
            EarlyMorning, // 6am - 9am
            Morning, // 9am - 12pm
            EarlyAfternoon, // 12pm - 3pm
            Afternoon, // 3pm - 6pm
            Evening, // 6pm - 9pm
            LateEvening, // 9pm - 12am
            Midnight, // 12am - 3am
            LateNight, // 3am - 6am
        }

        public int GetExperienceReward()
        {
            return _xpReward;
        }
        
        public CraftTimeOfDay GetCraftTimeOfDay()
        {
            return _craftTimeOfDay;
        }
        
        public Color GetPotionColor()
        {
            return _potionColor;
        }

        public List<RequiredMaterial> GetRequiredMaterials()
        {
            return _requiredMaterials;
        }

        public float GetCraftingTime()
        {
            return _craftingDuration;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is RecipeSO otherRecipe)
            {
                return otherRecipe.GetInstanceID() == GetInstanceID();
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GetInstanceID();
        }

        
        public bool IsMatch(List<ItemSO> items)
        {
            Debug.Log("Checking recipe...");
            // Create a new list to hold the quantities of each item
            List<RequiredMaterial> itemsCount = new List<RequiredMaterial>(_requiredMaterials);
    
            // For each item in the provided list...
            foreach (var item in items)
            {
                // ...check if the item is in the recipe
                for (int i = 0; i < itemsCount.Count; i++)
                {
                    if (itemsCount[i].requiredIngredient.GetItemID() == item.GetItemID())
                    {
                        // If the item is in the recipe, decrease the quantity in itemsCount
                        itemsCount[i] = new RequiredMaterial
                        {
                            requiredIngredient = itemsCount[i].requiredIngredient,
                            quantity = itemsCount[i].quantity - 1
                        };
                        break;
                    }
                }
            }
    
            // If any item in itemsCount still has a quantity greater than 0, return false
            foreach (var itemCount in itemsCount)
            {
                if (itemCount.quantity > 0)
                {
                    return false;
                }
            }
    
            // If we made it through the entire list without returning, then the items match the recipe
            return true;
        }

        public bool CanCraftAtThisTimeOfDay()
        {
            if (_craftTimeOfDay == CraftTimeOfDay.Any)
            {
                return true;
            }
    
            if (_craftTimeOfDay == CraftTimeOfDay.Day && TimeController.Instance.IsDayTime())
            {
                return true;
            }
    
            if (_craftTimeOfDay == CraftTimeOfDay.Night && TimeController.Instance.IsNightTime())
            {
                return true;
            }
            
            if (_craftTimeOfDay == CraftTimeOfDay.EarlyMorning && TimeController.Instance.CurrentTimeState == TimeController.TimeState.EarlyMorning)
            {
                return true;
            }
    
            if (_craftTimeOfDay == CraftTimeOfDay.Morning && TimeController.Instance.CurrentTimeState == TimeController.TimeState.Morning)
            {
                return true;
            }
            
            if (_craftTimeOfDay == CraftTimeOfDay.EarlyAfternoon && TimeController.Instance.CurrentTimeState == TimeController.TimeState.EarlyAfternoon)
            {
                return true;
            }
            
            if (_craftTimeOfDay == CraftTimeOfDay.Afternoon && TimeController.Instance.CurrentTimeState == TimeController.TimeState.Afternoon)
            {
                return true;
            }
            
            if (_craftTimeOfDay == CraftTimeOfDay.Evening && TimeController.Instance.CurrentTimeState == TimeController.TimeState.Evening)
            {
                return true;
            }
            
            if (_craftTimeOfDay == CraftTimeOfDay.LateEvening && TimeController.Instance.CurrentTimeState == TimeController.TimeState.LateEvening)
            {
                return true;
            }
            
            if (_craftTimeOfDay == CraftTimeOfDay.Midnight && TimeController.Instance.CurrentTimeState == TimeController.TimeState.Midnight)
            {
                return true;
            }
            
            if (_craftTimeOfDay == CraftTimeOfDay.LateNight && TimeController.Instance.CurrentTimeState == TimeController.TimeState.LateNight)
            {
                return true;
            }
    
            return false;
        }
    }
}

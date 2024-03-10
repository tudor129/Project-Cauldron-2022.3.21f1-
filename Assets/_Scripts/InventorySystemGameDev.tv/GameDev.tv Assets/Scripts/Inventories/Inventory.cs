using System;
using UnityEngine;
using GameDevTV.Saving;
using UnityEngine.Serialization;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// Provides storage for the player inventory. A configurable number of
    /// slots are available.
    ///
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Inventory : MonoBehaviour, ISaveable
    {
        // CONFIG DATA
        [FormerlySerializedAs("inventorySize")]
        [Tooltip("Allowed size")]
        [SerializeField] int _inventorySize = 16;

        // STATE
        InventorySlot[] _slots;

        public struct InventorySlot
        {
            public ItemSO ItemSo;
            public int number;
        }

        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action inventoryUpdated;

        /// <summary>
        /// Convenience for getting the player's inventory.
        /// </summary>
        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        /// <summary>
        /// Could this item fit anywhere in the inventory?
        /// </summary>
        public bool HasSpaceFor(ItemSO itemSo)
        {
            return FindSlot(itemSo) >= 0;
        }

        /// <summary>
        /// How many slots are in the inventory?
        /// </summary>
        public int GetSize()
        {
            return _slots.Length;
        }

        /// <summary>
        /// Attempt to add the items to the first available slot.
        /// </summary>
        /// <param name="itemSo">The item to add.</param>
        /// <param name="number">The number to add.</param>
        /// <returns>Whether or not the item could be added.</returns>
        public bool AddToFirstEmptySlot(ItemSO itemSo, int number)
        {
            int i = FindSlot(itemSo);

            if (i < 0)
            {
                return false;
            }

            _slots[i].ItemSo = itemSo;
            _slots[i].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(ItemSO itemSo)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (object.ReferenceEquals(_slots[i].ItemSo, itemSo))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public ItemSO GetItemInSlot(int slot)
        {
            return _slots[slot].ItemSo;
        }

        /// <summary>
        /// Get the number of items in the given slot.
        /// </summary>
        public int GetNumberInSlot(int slot)
        {
            return _slots[slot].number;
        }

        /// <summary>
        /// Remove a number of items from the given slot. Will never remove more
        /// that there are.
        /// </summary>
        public void RemoveFromSlot(int slot, int number)
        {
            _slots[slot].number -= number;
            if (_slots[slot].number <= 0)
            {
                _slots[slot].number = 0;
                _slots[slot].ItemSo = null;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }

        /// <summary>
        /// Will add an item to the given slot if possible. If there is already
        /// a stack of this type, it will add to the existing stack. Otherwise,
        /// it will be added to the first empty slot.
        /// </summary>
        /// <param name="slot">The slot to attempt to add to.</param>
        /// <param name="itemSo">The item type to add.</param>
        /// <param name="number">The number of items to add.</param>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddItemToSlot(int slot, ItemSO itemSo, int number)
        {
            if (_slots[slot].ItemSo != null)
            {
                return AddToFirstEmptySlot(itemSo, number); ;
            }

            var i = FindStack(itemSo);
            if (i >= 0)
            {
                slot = i;
            }

            _slots[slot].ItemSo = itemSo;
            _slots[slot].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        // PRIVATE

        void Awake()
        {
            _slots = new InventorySlot[_inventorySize];
        }

        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <returns>-1 if no slot is found.</returns>
        int FindSlot(ItemSO itemSo)
        {
            int i = FindStack(itemSo);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        /// <summary>
        /// Find an empty slot.
        /// </summary>
        /// <returns>-1 if all slots are full.</returns>
        int FindEmptySlot()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].ItemSo == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Find an existing stack of this item type.
        /// </summary>
        /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
        int FindStack(ItemSO itemSo)
        {
            if (!itemSo.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (object.ReferenceEquals(_slots[i].ItemSo, itemSo))
                {
                    return i;
                }
            }
            return -1;
        }

        [System.Serializable]
        struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }
    
        object ISaveable.CaptureState()
        {
            var slotStrings = new InventorySlotRecord[_inventorySize];
            for (int i = 0; i < _inventorySize; i++)
            {
                if (_slots[i].ItemSo != null)
                {
                    slotStrings[i].itemID = _slots[i].ItemSo.GetItemID();
                    slotStrings[i].number = _slots[i].number;
                }
            }
            return slotStrings;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < _inventorySize; i++)
            {
                _slots[i].ItemSo = ItemSO.GetFromID(slotStrings[i].itemID);
                _slots[i].number = slotStrings[i].number;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }
    }
}
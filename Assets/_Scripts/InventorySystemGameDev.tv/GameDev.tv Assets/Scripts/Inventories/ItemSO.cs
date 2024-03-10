using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// A ScriptableObject that represents any item that can be put in an
    /// inventory.
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as `ActionItem` or
    /// `EquipableItem`.
    /// </remarks>
    public abstract class ItemSO : ScriptableObject, ISerializationCallbackReceiver
    {
        // CONFIG DATA
        [FormerlySerializedAs("itemID")]
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] string _itemID = null;
        [FormerlySerializedAs("displayName")]
        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField] string _displayName = null;
        [FormerlySerializedAs("description")]
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] string _description = null;
        [FormerlySerializedAs("icon")]
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField] Sprite _icon = null;
        [FormerlySerializedAs("pickup")]
        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField] Pickup _pickup = null;
        [FormerlySerializedAs("stackable")]
        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField] bool _stackable = false;
        public enum DropBehaviorType {Default, FloatDown, FloatTowards}

        // STATE
        static Dictionary<string, ItemSO> _itemLookupCache;

        // PUBLIC

        /// <summary>
        /// Get the inventory item instance from its UUID.
        /// </summary>
        /// <param name="itemID">
        /// String UUID that persists between game instances.
        /// </param>
        /// <returns>
        /// Inventory item instance corresponding to the ID.
        /// </returns>
        public static ItemSO GetFromID(string itemID)
        {
            if (_itemLookupCache == null)
            {
                _itemLookupCache = new Dictionary<string, ItemSO>();
                var itemList = Resources.LoadAll<ItemSO>("");
                foreach (var item in itemList)
                {
                    if (_itemLookupCache.ContainsKey(item._itemID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate GameDevTV.UI.InventorySystem ID for objects: {0} and {1}", _itemLookupCache[item._itemID], item));
                        continue;
                    }

                    _itemLookupCache[item._itemID] = item;
                }
            }

            if (itemID == null || !_itemLookupCache.ContainsKey(itemID)) return null;
            return _itemLookupCache[itemID];
        }
        
        /// <summary>
        /// Spawn the pickup gameobject into the world.
        /// </summary>
        /// <param name="position">Where to spawn the pickup.</param>
        /// <param name="number">How many instances of the item does the pickup represent.</param>
        /// <returns>Reference to the pickup object spawned.</returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(this._pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }
        
        public DropBehaviorType DropBehavior;
 
        public Sprite GetIcon()
        {
            return _icon;
        }

        public string GetItemID()
        {
            return _itemID;
        }

        public bool IsStackable()
        {
            return _stackable;
        }
        
        public string GetDisplayName()
        {
            return _displayName;
        }

        public string GetDescription()
        {
            return _description;
        }

        // PRIVATE
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(_itemID))
            {
                _itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }
    }
}

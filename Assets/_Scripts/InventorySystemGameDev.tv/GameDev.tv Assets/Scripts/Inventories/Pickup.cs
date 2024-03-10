using InventoryExample.Control;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// To be placed at the root of a Pickup prefab. Contains the data about the
    /// pickup such as the type of item and the number.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        // STATE
        [FormerlySerializedAs("itemSo")] [FormerlySerializedAs("item")] [SerializeField] ItemSO _itemSo;
        int _number = 1;

        // CACHED REFERENCE
        Inventory _inventory;

        // LIFECYCLE METHODS

        void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _inventory = player.GetComponent<Inventory>();
        }

        // PUBLIC

        /// <summary>
        /// Set the vital data after creating the prefab.
        /// </summary>
        /// <param name="itemSo">The type of item this prefab represents.</param>
        /// <param name="number">The number of items represented.</param>
        public void Setup(ItemSO itemSo, int number)
        {
            this._itemSo = itemSo;
            if (!itemSo.IsStackable())
            {
                number = 1;
            }
            this._number = number;
        }

        public ItemSO GetItem()
        {
            return _itemSo;
        }

        public int GetNumber()
        {
            return _number;
        }

        public void PickupItem()
        {
            bool foundSlot = _inventory.AddToFirstEmptySlot(_itemSo, _number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            return _inventory.HasSpaceFor(_itemSo);
        }
    }
}
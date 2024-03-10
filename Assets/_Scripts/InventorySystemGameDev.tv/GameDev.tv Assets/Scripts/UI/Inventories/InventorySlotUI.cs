using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameDevTV.Inventories;
using GameDevTV.Core.UI.Dragging;

namespace GameDevTV.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<GameDevTV.Inventories.ItemSO>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;

        // STATE
        int index;
        GameDevTV.Inventories.ItemSO _itemSo;
        Inventory inventory;

        // PUBLIC

        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public int MaxAcceptable(GameDevTV.Inventories.ItemSO itemSo)
        {
            if (inventory.HasSpaceFor(itemSo))
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(GameDevTV.Inventories.ItemSO itemSo, int number)
        {
            inventory.AddItemToSlot(index, itemSo, number);
        }

        public GameDevTV.Inventories.ItemSO GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }
    }
}
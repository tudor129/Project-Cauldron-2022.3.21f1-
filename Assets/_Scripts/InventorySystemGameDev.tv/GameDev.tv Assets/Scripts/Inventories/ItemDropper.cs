using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using System.Collections;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// To be placed on anything that wishes to drop pickups into the world.
    /// Tracks the drops for saving and restoring.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        // STATE
        List<Pickup> _droppedItems = new List<Pickup>();

        // PUBLIC

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="itemSo">The item type for the pickup.</param>
        /// <param name="number">
        /// The number of items contained in the pickup. Only used if the item
        /// is stackable.
        /// </param>
        public void DropItem(ItemSO itemSo, int number)
        {
            SpawnPickup(itemSo, GetDropLocation(), number);
        }

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="itemSo">The item type for the pickup.</param>
        public void DropItem(ItemSO itemSo)
        {
            SpawnPickup(itemSo, GetDropLocation(), 1);
        }

        // PROTECTED

        /// <summary>
        /// Override to set a custom method for locating a drop.
        /// </summary>
        /// <returns>The location the drop should be spawned.</returns>
        protected virtual Vector3 GetDropLocation()
        {
             // return transform.position - transform.forward;
             
             // Find the CameraTransparencyController in the scene
             
             InteractionRaycaster interactionRaycaster = FindObjectOfType<InteractionRaycaster>();
             

             return interactionRaycaster.GetMouseWorldPosition();
        }
        
        public Vector3 GetDropLocationForPickup()
        {
            return GetDropLocation();
        }

        // PRIVATE

        public Pickup SpawnPickup(ItemSO itemSo, Vector3 spawnLocation, int number)
        {
            Vector3 initialSpawnLocation = new Vector3(spawnLocation.x, spawnLocation.y + 1, spawnLocation.z);
            var pickup = itemSo.SpawnPickup(spawnLocation, number);
            float duration = 2f;
            
            /*switch (itemSo.DropBehavior)
            {
                case ItemSO.DropBehaviorType.Default:
                    
                    pickup.transform.DOMove(spawnLocation, duration).SetEase(Ease.OutQuad);

                    break;

                case ItemSO.DropBehaviorType.FloatDown:
                    
                    pickup.transform.DOMove(initialSpawnLocation, duration).SetEase(Ease.OutQuad);
                    break;

                case ItemSO.DropBehaviorType.FloatTowards:
                    // apply FloatTowards behavior
                    break;
            }*/

            _droppedItems.Add(pickup);

            return pickup;

        }

        [System.Serializable]
        struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new DropRecord[_droppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i].itemID = _droppedItems[i].GetItem().GetItemID();
                droppedItemsList[i].position = new SerializableVector3(_droppedItems[i].transform.position);
                droppedItemsList[i].number = _droppedItems[i].GetNumber();
            }
            return droppedItemsList;
        }

        void ISaveable.RestoreState(object state)
        {
            var droppedItemsList = (DropRecord[])state;
            foreach (var item in droppedItemsList)
            {
                var pickupItem = ItemSO.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                int number = item.number;
                SpawnPickup(pickupItem, position, number);
            }
        }

        /// <summary>
        /// Remove any drops in the world that have subsequently been picked up.
        /// </summary>
        void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in _droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            _droppedItems = newList;
        }
    }
}
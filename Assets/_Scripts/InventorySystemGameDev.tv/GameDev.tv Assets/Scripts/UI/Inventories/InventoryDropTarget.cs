using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// Handles spawning pickups when item dropped into the world.
    /// 
    /// Must be placed on the root canvas where items can be dragged. Will be
    /// called if dropped over empty space. 
    /// </summary>
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<GameDevTV.Inventories.ItemSO>
    {
        public void AddItems(GameDevTV.Inventories.ItemSO itemSo, int number)
        {
            Debug.Log("I dropped " + itemSo.name);
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().DropItem(itemSo, number);
         }
        
        public int MaxAcceptable(GameDevTV.Inventories.ItemSO itemSo)
        {
            return int.MaxValue;
        }
        
       
    }
}
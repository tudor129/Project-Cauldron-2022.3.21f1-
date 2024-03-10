using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An inventory item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// This class should be used as a base. Subclasses must implement the `Use`
    /// method.
    /// </remarks>
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Action Item"))]
    public class ActionItemSo : ItemSO
    {
        // CONFIG DATA
        [FormerlySerializedAs("consumable")]
        [Tooltip("Does an instance of this item get consumed every time it's used.")]
        [SerializeField] bool _consumable = false;

        // PUBLIC

        /// <summary>
        /// Trigger the use of this item. Override to provide functionality.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual void Use(GameObject user)
        {
            Debug.Log("Using action: " + this);
        }

        public bool isConsumable()
        {
            return _consumable;
        }
    }
}
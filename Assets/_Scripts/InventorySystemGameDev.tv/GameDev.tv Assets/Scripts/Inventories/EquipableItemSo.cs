using UnityEngine;
using UnityEngine.Serialization;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Equipable Item"))]
    public class EquipableItemSo : ItemSO
    {
        // CONFIG DATA
        [FormerlySerializedAs("allowedEquipLocation")]
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation _allowedEquipLocation = EquipLocation.Weapon;

        // PUBLIC

        public EquipLocation GetAllowedEquipLocation()
        {
            return _allowedEquipLocation;
        }
    }
}
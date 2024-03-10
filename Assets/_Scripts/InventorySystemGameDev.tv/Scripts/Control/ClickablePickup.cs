using UnityEngine;
using GameDevTV.Inventories;
using InventoryExample.Control;

namespace InventoryExample.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour
    {
        Pickup pickup;
        

        void Awake()
        {
            pickup = GetComponent<Pickup>();
            
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool HandleRaycast(InteractionRaycaster callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //pickup.PickupItem();
                if (Player.Instance.IsHoldingObject())
                {
                    return false;
                }
                transform.SetParent(Player.Instance.GetObjectHolder());
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                
                Player.Instance.HoldObject(pickup); // Notify the player that it's holding an object
            }
            return true;
        }
    }
}
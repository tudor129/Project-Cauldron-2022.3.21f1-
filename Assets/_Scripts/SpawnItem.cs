using GameDevTV.Inventories;
using InventoryExample.Control;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpawnItem : MonoBehaviour
{
    Button _button; 
    Pickup _spawnedObject;
    
    void Awake()
    {
        
    }

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ToggleObject);
    }

    void Update()
    {
        
    }

    void ToggleObject()
    {
        // If an object has been spawned, destroy it and set spawnedObject to null
        if (_spawnedObject != null)
        {
            Player.Instance.HoldObject(_spawnedObject);
            
            _spawnedObject = null;
        }
        // If no object has been spawned, spawn one and store the reference in spawnedObject
        else
        {
            // Get the current held item from the player
            GameDevTV.Inventories.ItemSO currentItemSO = Player.Instance.GetCurrentHeldItem();
            if (currentItemSO == null)
            {
                return;
            }
            
            // Get the button's world position
            Vector3 buttonWorldPosition = _button.transform.position;
            
            // Spawn the 3D object at the button's world position with the desired rotation
            _spawnedObject = currentItemSO.SpawnPickup(buttonWorldPosition, 1);
            Player.Instance.ReleaseCurrentHeldItem();
        }
    }
}

using GameDevTV.Inventories;
using InventoryExample.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunoverPickup : MonoBehaviour, IObjectParent
{
    static List<RunoverPickup> _allRunoverPickups = new List<RunoverPickup>();

    Animator _animator;
    
    float _proximityThreshold = 3f;

    void Awake()
    {
        _allRunoverPickups.Add(this);
    }
    
    IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("No Animator found on " + gameObject.name + ". Please add one.");
            yield break; // Exit the Coroutine if there's no Animator
        }
        
        while (true)
        {
            RunoverPickup[] runoverPickups = RunoverPickup.GetAllPickups();
            GameObject[] runoverPickupGameObjects = runoverPickups.Select(rp => rp.gameObject).ToArray();
        
            GameObject closestRunoverPickup = Player.Instance.IsCloseToObject(runoverPickupGameObjects, _proximityThreshold);

            if (gameObject == closestRunoverPickup && !Player.Instance.IsHoldingObject())
            {
                _animator.SetBool("IsPlayerClose", true);
            }
            else
            {
                _animator.SetBool("IsPlayerClose", false);
            }
        
            yield return new WaitForSeconds(0.2f); // check every 0.2 seconds
        }
    }

    void OnDestroy()
    {
        _allRunoverPickups.Remove(this);

    }
    
    public static RunoverPickup[] GetAllPickups() 
    {
        return _allRunoverPickups.ToArray();
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null && !player.IsHoldingObject())
        {
            Transform objectHolder = player.GetObjectHolder();
            
            
            if (objectHolder != null)
            {
                Pickup pickup = GetComponent<Pickup>();
                if (pickup != null)
                {
                    pickup.enabled = false;
                }
                Collider collider = GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
                
                transform.SetParent(objectHolder);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                
                player.HoldObject(pickup); // Notify the player that it's holding an object
            }
        }
    }

}

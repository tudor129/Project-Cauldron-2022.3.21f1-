using System;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObjectsController : MonoBehaviour
{
    public event Action<GameObject> OnObjectInTheWay;
    public event Action<GameObject> OnObjectNotInTheWay;
    
    [SerializeField] LayerMask _obstacleLayer;
    
    List<GameObject> _objectsInTheWay = new List<GameObject>();
    
    GameObject _currentBlockingObject;
    
    void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == _obstacleLayer.value)
        {
            _objectsInTheWay.Add(other.gameObject);
            OnObjectInTheWay?.Invoke(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer) == _obstacleLayer.value)
        {
            _objectsInTheWay.Remove(other.gameObject);
            OnObjectNotInTheWay?.Invoke(other.gameObject);
        }
    }
    
    /*void Update()
    {
        // Find the closest object that's currently in the way
        GameObject closestObjectInTheWay = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject obj in _objectsInTheWay)
        {
            float distance = (obj.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObjectInTheWay = obj;
            }
        }

        // If there's an object in the way, and it's not the same as the last object, notify listeners
        if (closestObjectInTheWay != null && closestObjectInTheWay != _currentBlockingObject)
        {
            OnObjectInTheWay?.Invoke(closestObjectInTheWay);
        }

        // If there's no object in the way, but there was one before, notify listeners
        if (closestObjectInTheWay == null && _currentBlockingObject != null)
        {
            OnObjectNotInTheWay?.Invoke(_currentBlockingObject);
        }

        // Update the current blocking object
        _currentBlockingObject = closestObjectInTheWay;
    }*/
}

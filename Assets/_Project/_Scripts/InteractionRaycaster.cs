using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionRaycaster : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayer;

    GameObject _currentBlockingObject;
    Camera _camera;
    
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        PerformRaycast();
        MouseWorldPosition();
    }
    Vector3 MouseWorldPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            return hit.point;
        }

        return Vector3.zero; // Return an invalid position if the raycast doesn't hit the ground
    }
    
    void PerformRaycast()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // ClickablePickup clickablePickup = hit.collider.GetComponent<ClickablePickup>();
            // if (clickablePickup != null)
            // {
            //     clickablePickup.HandleRaycast(this);
            // }
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        return MouseWorldPosition();
    }
}

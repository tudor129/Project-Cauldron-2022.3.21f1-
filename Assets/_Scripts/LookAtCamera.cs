using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera _camera;
    
    void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    void Start()
    {
        _camera = Camera.main;
    }


    void Update()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
            _camera.transform.rotation * Vector3.up);
    }
}

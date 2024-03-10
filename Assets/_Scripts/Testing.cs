using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        //_camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.LookAt(_camera.transform.position);
    }
}

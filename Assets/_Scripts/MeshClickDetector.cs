using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MeshClickDetector : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] Camera _cameraPrefab;
    float distanceFromShelf = 2f;

    Camera _spawnedCamera;
    int _mainCameraOriginalEventMask;
    void Start()
    {
        //_mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            // Create a ray from the camera through the mouse position
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            // Check if the ray intersects with the collider on this GameObject
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Mesh clicked: " + gameObject.name);
                if (_spawnedCamera == null)
                {
                    Vector3 shelfPosition = transform.position;
                    Vector3 cameraSpawnPosition = shelfPosition - transform.forward * -distanceFromShelf + Vector3.up * 2f;
                    Quaternion spawnRotation = Quaternion.Euler(18.16f, 0, 0);

                    Camera cameraObject = Instantiate(_cameraPrefab, cameraSpawnPosition, spawnRotation);
                    _spawnedCamera = cameraObject.GetComponent<Camera>();

                    // Set the spawned camera to be the active camera
                    _spawnedCamera.enabled = true;
                    _mainCamera.eventMask = 0;
                }
            }
            else
            {
                //If a camera has been spawned, disable it and destroy it
                if (_spawnedCamera != null)
                {
                    _spawnedCamera.enabled = false;
                    Destroy(_spawnedCamera.gameObject);
                    //_mainCamera.eventMask = -1;
                    _spawnedCamera = null;
                }
            }
        }
    }
}

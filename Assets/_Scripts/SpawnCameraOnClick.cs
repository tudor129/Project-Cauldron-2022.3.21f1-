using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class SpawnCameraOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject _cameraPrefab; // Assign your Camera prefab in the Inspector
    public float _distanceFromShelf = 2f; // Adjust the distance between the camera and the shelf
    private Camera _spawnedCamera;

    public void OnPointerClick(PointerEventData eventData)
    {
        // If a camera has not been spawned, spawn one in front of the shelf
        if (_spawnedCamera == null)
        {
            Vector3 shelfPosition = transform.position;
            Vector3 cameraSpawnPosition = shelfPosition - transform.forward * _distanceFromShelf;

            GameObject cameraObject = Instantiate(_cameraPrefab, cameraSpawnPosition, Quaternion.identity);
            _spawnedCamera = cameraObject.GetComponent<Camera>();

            // Set the spawned camera to be the active camera
            _spawnedCamera.enabled = true;
        }
        // If a camera has been spawned, destroy it and set spawnedCamera to null
        else
        {
            Destroy(_spawnedCamera.gameObject);
            _spawnedCamera = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public static readonly float TILE_SIZE = 1000f;
    
    public Vector3 WorldPosition { get; private set; }
    
    GameObject _tileGameObject;
    List<GameObject> _tileProps = new List<GameObject>();
    
    GameObject[] _props;
    
    
    public Tile(Vector3 worldPosition, GameObject[] props, GameObject tilePrefab)
    {
        WorldPosition = worldPosition;
        _props = props;
        
        // Instantiate the actual tile GameObject
        
        _tileGameObject = GameObject.Instantiate(tilePrefab, WorldPosition, Quaternion.identity);
        _tileGameObject.transform.localScale = new Vector3(TILE_SIZE / 10, 1f, TILE_SIZE / 10);
        
    }
    
    public void PlaceProps(int minProps, int maxProps)
    {
        
        int numberOfProps = Random.Range(minProps, maxProps);
        Vector3 adjustedWorldPosition = new Vector3(WorldPosition.x + TILE_SIZE * 0.5f, 0f, WorldPosition.z + TILE_SIZE * 0.5f);

        for (int i = 0; i < numberOfProps; i++)
        {
            GameObject propToPlace = _props[Random.Range(0, _props.Length)];
            int tries = 0;
            bool placed = false;

            Vector3 position = new Vector3(
                Random.Range(adjustedWorldPosition.x, adjustedWorldPosition.x - TILE_SIZE),
                0f, 
                Random.Range(adjustedWorldPosition.z, adjustedWorldPosition.z - TILE_SIZE)
            );
            
            GameObject propInstance = GameObject.Instantiate(propToPlace, position, Quaternion.identity);
            propInstance.transform.SetParent(_tileGameObject.transform);
            _tileProps.Add(propInstance);
           
        }
    }
    
    bool IsLocationFree(Vector3 position, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        if (hitColliders.Length > 0)
        {
            return false;  // Location is not free
        }
        return true;  // Location is free
    }
    
    public void ClearProps()
    {
        foreach (GameObject prop in _tileProps)
        {
            GameObject.Destroy(prop);
        }
        _tileProps.Clear();
        
        GameObject.Destroy(_tileGameObject);
    }

    public void Deactivate()
    {
        
        foreach (GameObject prop in _tileProps)
        {
            prop.SetActive(false);
        }
        _tileGameObject.SetActive(false);
    }
    public void Activate()
    {
        
        foreach (GameObject prop in _tileProps)
        {
            prop.SetActive(true);
        }
        _tileGameObject.SetActive(true);
    }
}

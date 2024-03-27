using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    TileManager _tileManager;
    
    Transform _spawnPoint;

    void Start()
    {
        if (_tileManager == null)
        {
            _tileManager = FindObjectOfType<TileManager>();
        }
    }
    
    Dictionary<string, string> _spawnPointNames = new Dictionary<string, string>()
    {
        {"TopTrigger", "UpSpawnPoint"},
        {"DownTrigger", "DownSpawnPoint"},
        {"LeftTrigger", "LeftSpawnPoint"},
        {"RightTrigger", "RightSpawnPoint"},
        {"Left-UpTrigger", "Left-UpSpawnPoint"},
        {"Left-DownTrigger", "Left-DownSpawnPoint"},
        {"Right-UpTrigger", "Right-UpSpawnPoint"},
        {"Right-DownTrigger", "Right-DownSpawnPoint"}
    };
  
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _spawnPointNames.ContainsKey(gameObject.tag))
        {
            TrySpawnTile(_spawnPointNames[gameObject.tag]);
        }
    }
    // There is room for optimization here
    // We could store the spawn points in a dictionary
    // and then just check if the key exists
    // and if it does, use the value as the spawn point name
    void TrySpawnTile(string spawnPointName)
    {
        foreach (Transform child in transform.parent)
        {
            if (child.name == spawnPointName)
            {
                _spawnPoint = child;
                Vector3 spawnPosition = _spawnPoint.position;
                
                if (!IsTileAtPosition(spawnPosition))
                {
                    Tile newTile = new Tile(spawnPosition, _tileManager._propsPrefabs, _tileManager._tilePrefab);
                    _tileManager.AddTile(newTile);
                    newTile.PlaceProps(_tileManager.GetMinProps(), _tileManager.GetMaxProps());
                }
                break;
            }
        }
    }
    
    bool IsTileAtPosition(Vector3 position)
    {
        foreach (Tile tile in _tileManager._activeTiles)
        {
            if (tile.WorldPosition == position)
            {
                return true;
            }
        }
        return false;
    }
}

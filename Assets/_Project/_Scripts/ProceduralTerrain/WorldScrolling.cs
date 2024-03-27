using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScrolling : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    
    [SerializeField] int _terrainTileHorizontalCount;
    [SerializeField] int _terrainTileVerticalCount;
    [SerializeField] float _tileSize = 10f;

    [SerializeField] int _fieldOfVisionHeight = 3;
    [SerializeField] int _fieldOfVisionWidth = 3;
    
    Vector2Int _onTileGridPlayerPosition;
    Vector2Int _currentTilePosition = new Vector2Int(0, 0);
    Vector2Int _currentPlayerTilePosition;
    [SerializeField] Vector2Int _playerTilePosition;

    GameObject[,] _terrainTiles;
    

    void Awake()
    {
        _terrainTiles = new GameObject[_terrainTileHorizontalCount, _terrainTileVerticalCount];
    }

    void Update()
    {
        _playerTilePosition.x = (int)(_playerTransform.position.x / _tileSize);
        _playerTilePosition.y = (int)(_playerTransform.position.z / _tileSize);

        _playerTilePosition.x -= _playerTransform.position.x < 0 ? 1 : 0;
        _playerTilePosition.y -= _playerTransform.position.y < 0 ? 1 : 0;
        
        if (_currentTilePosition != _playerTilePosition)
        {
            _currentTilePosition = _playerTilePosition;
        
            _onTileGridPlayerPosition.x = CalculatePositionOnAxis(_playerTransform.position.x, true);
            _onTileGridPlayerPosition.y = CalculatePositionOnAxis(_playerTransform.position.z, false);
            UpdateTilesOnScreen();
        }
        
       
    }
    void UpdateTilesOnScreen()
    {
        for (int pov_x = -(_fieldOfVisionWidth / 2); pov_x <= _fieldOfVisionWidth / 2; pov_x++)
        {
            for (int pov_y = -(_fieldOfVisionHeight / 2); pov_y <= _fieldOfVisionHeight / 2; pov_y++)
            {
                int tileToUpdate_x = CalculatePositionOnAxis(_playerTilePosition.x + pov_x, true);
                int tileToUpdate_y = CalculatePositionOnAxis(_playerTilePosition.y + pov_y, false);
                
                
                GameObject tileToUpdate = _terrainTiles[tileToUpdate_x, tileToUpdate_y];
                if (tileToUpdate != null)
                {
                    tileToUpdate.transform.position = CalculateTilePosition(_playerTilePosition.x + pov_x, _playerTilePosition.y + pov_y);
                }
                else
                {
                    Debug.LogError($"Tile at index ({tileToUpdate_x}, {tileToUpdate_y}) is null!");
                }
            }
        }
    }

    Vector3 CalculateTilePosition(int x, int y)
    {
        return new Vector3(x * _tileSize,  0, y * _tileSize);
    }
    
    int CalculatePositionOnAxis(float currentValue, bool horizontal)
    {
        if (horizontal)
        {
            if (currentValue >= 0)
            {
                currentValue = currentValue % _terrainTileHorizontalCount;
            }
            else
            {
                currentValue += 1;
                currentValue = _terrainTileHorizontalCount - 1 + currentValue % _terrainTileHorizontalCount;
            }
        }
        else
        {
            if (currentValue >= 0)
            {
                currentValue = currentValue % _terrainTileVerticalCount;
            }
            else
            {
                currentValue += 1;
                currentValue = _terrainTileVerticalCount - 1 + currentValue % _terrainTileVerticalCount;
            }
        }
        Debug.Log("Current value: " + currentValue);

        return (int)currentValue;
    }


    public void Add(GameObject tileGameObject, Vector2Int tilePosition)
    {
        _terrainTiles[tilePosition.x, tilePosition.y] = tileGameObject;
    }
}

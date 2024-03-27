using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public delegate void TileEventHandler(Tile newTile);
    public event TileEventHandler OnTileCreated;
    
    public GameObject _tilePrefab;
    public GameObject[] _propsPrefabs;

    public List<Tile> _activeTiles = new List<Tile>();

    [SerializeField] Transform _player;
 
    [SerializeField] int _minProps = 5;
    [SerializeField] int _maxProps = 10;
    
    [Header("This should not be lower than 110")]
    const float DisableDistance = 1100f;


    void Start()
    {
        Vector3 initialPos = new Vector3(0, 0, 0);

        Vector3 playerTilePos = new Vector3(
            Mathf.Floor(_player.position.x / Tile.TILE_SIZE) * Tile.TILE_SIZE,
            0f,
            Mathf.Floor(_player.position.z / Tile.TILE_SIZE) * Tile.TILE_SIZE);

        Tile initialTile = new Tile(initialPos, _propsPrefabs, _tilePrefab);
        initialTile.PlaceProps(_minProps, _maxProps);
        AddTile(initialTile);

        OnTileCreated += TileChecker;
        StartCoroutine(PeriodicTileCheck());
    }

    void Update()
    {
       
    }

    public void AddTile(Tile tile)
    {
        _activeTiles.Add(tile);
        
        OnTileCreated?.Invoke(tile);
    }
    
    IEnumerator PeriodicTileCheck()
    {
        while (true)
        {
            foreach (Tile tile in _activeTiles)
            {
                TileChecker(tile);
            }
            yield return new WaitForSeconds(0.5f); 
        }
    }

    void TileChecker(Tile newTile)
    {
        foreach (Tile tile in _activeTiles)
        {
            float distanceToPlayer = Vector3.Distance(tile.WorldPosition, _player.position);
            
            if (distanceToPlayer > DisableDistance)
            {
                // Assuming Tile has a method or property to disable it
                tile.Deactivate();
            }
            else
            {
                tile.Activate();
            }
        }
    }

    public int GetMinProps()
    {
        return _minProps;
    }
    public int GetMaxProps()
    {
        return _maxProps;
    }
}
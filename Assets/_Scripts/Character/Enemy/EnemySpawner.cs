using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    
    [Header("If this is not assigned, the spawner will not work")]
    [SerializeField] Transform _playerTransform; // if this transform is not assigned, the spawner will not work
    [SerializeField] Transform _spawnHeightTransform;
    [SerializeField] Camera _mainCamera;
    [SerializeField] List<EnemySO> _enemyDataList;
    [Tooltip("The area around the player that enemies can spawn in")]
    [SerializeField] Vector3 _spawnArea;
    [SerializeField] float _singleEnemySpawnInte = 5f;
    [SerializeField] float _formationSpawnInterval = 60f;
    
    [Header("Square Formation")]
    [SerializeField, Range(0, 10)] int _unitWidth = 5;
    [SerializeField, Range(0, 10)] int _unitDepth = 5;
    [SerializeField, Range(0.1f, 5)] int _spread = 1;
    [SerializeField, Range(-5, 5)] int _nthOffset = 0;
    [SerializeField, Range(0, 1)] int _noise = 0;
    [SerializeField] bool _hollow;
    
    float _viewspaceOffset = 0.2f; // the amount to offset the spawn position in viewspace coordinates

    Vector3[][] _formations;
    float _formationSpacing = 2f; // the distance between enemies in a formation
    int _maxEnemies = 70;
    float _timer;
    
    public enum FormationType
    {
        Square,
        Fibonacci
    }
    
    FormationType _currentFormation = FormationType.Square;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        IEnumerable<Vector3> _squareFormationList = GenerateSquareFormation();
        
        _formations = new Vector3[][]
        {
            _squareFormationList.ToArray(),
        };
    }

    void Start()
    {
        CoroutineManager.Instance.StartManagedCoroutine(SpawnWavesCoroutine());
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _singleEnemySpawnInte)
        {
            // Reset the timer
            _timer = 0f;

            // Spawn the enemy
            SpawnEnemy();
        }
        if (_timer >= _formationSpawnInterval)
        {
            // Reset the timer
            _timer = 0f;

            // Spawn the enemy
            //StartCoroutine(SpawnWavesCoroutine());
        }
    }

    IEnumerable<Vector3> GenerateSquareFormation()
    {
        _currentFormation = FormationType.Square;
        var middleOffset = new Vector3(_unitWidth * 0.5f, 0, _unitDepth * 0.5f);
        
        for (int x = 0; x < _unitWidth; x++)
        {
            for (int z = 0; z < _unitDepth; z++)
            {
                if (_hollow && x != 0 && x != _unitWidth - 1 && z != 0 && z != _unitDepth - 1)
                {
                    continue;
                }

                var pos = new Vector3(x + (z % 2 == 0 ? 0 : _nthOffset), 0, z);
                
                pos -= middleOffset;
                
                pos *= _spread;

                pos += GetNoise(pos);

                yield return pos;
            }
        }
    }

    Vector3 GetNoise(Vector3 pos)
    {
        var noise = Mathf.PerlinNoise(pos.x * _noise, pos.z * _noise);
        
        return new Vector3(noise, 0, noise);
    }
    
    IEnumerator SpawnWavesCoroutine()
    {
        while (true) // Repeat forever
        {
            if (Enemy.ActiveEnemies.Count < _maxEnemies)
            {
                // Choose a random formation
                Vector3[] formation = _formations[Random.Range(0, _formations.Length)];

                // Spawn a wave at a random position around the player
                Vector3 wavePosition = GetSquareFormationSpawnPosition();
                
                Vector3 spawnHeight = GetSpawnHeight();
                
                SpawnWave(wavePosition, formation);

                // Wait for x seconds before spawning the next wave
                yield return new WaitForSeconds(_formationSpawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
                
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetSingleEnemySpawnPosition();
        EnemySO enemySO = _enemyDataList[0];

        var enemyObject = ObjectPoolManager.SpawnObject(enemySO.EnemyPrefab, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
        var enemy = enemyObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.SetPlayerTransform(_playerTransform);
            enemy.SetEnemyData(enemySO);
        }
    }
    
    void SpawnWave(Vector3 wavePosition, Vector3[] formation)
    {
        foreach (Vector3 offset in formation)
        {
            Vector3 spawnPosition = wavePosition + offset;
            EnemySO enemySO = _enemyDataList[1];
            var enemyObject = ObjectPoolManager.SpawnObject(enemySO.EnemyPrefab, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
            var enemy = enemyObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.SetPlayerTransform(_playerTransform);
                enemy.SetEnemyData(enemySO);
            }
        }
    }
    
    Vector3 GetSquareFormationSpawnPosition()
    {
        Vector3 spawnPos = new Vector3();
        
        spawnPos += _playerTransform.transform.position;

        // Ensure the enemy spawns on the ground plane
        spawnPos.y = _spawnHeightTransform.position.y;

        return spawnPos;
    }

    Vector3 GetSingleEnemySpawnPosition()
    {
        Vector3 spawnPos = GenerateRandomPosition();
        
        spawnPos += _playerTransform.transform.position;

        // Ensure the enemy spawns on the ground plane
        spawnPos.y = _spawnHeightTransform.position.y;

        return spawnPos;
    }

    Vector3 GetSpawnHeight()
    {
        Vector3 spawnHeight = _spawnHeightTransform.position;
        spawnHeight += _playerTransform.transform.position;
        spawnHeight.y = _spawnHeightTransform.position.y;
        
        
        return spawnHeight;
    }

    Vector3 GenerateRandomPosition()
    {
        Vector3 position = new Vector3();
        
        float f = Random.value > 0.5f ? -1 : 1;
        if (Random.value > 0.5)
        {
            position.x = Random.Range(-_spawnArea.x, _spawnArea.x);
            position.z = _spawnArea.z * f;
        }
        else
        {
            position.z = Random.Range(-_spawnArea.z, _spawnArea.z);
            position.x = _spawnArea.x * f;
        }
        
        return position;
    }
    public FormationType GetFormation()
    {
        return _currentFormation;
    }
   
    
}

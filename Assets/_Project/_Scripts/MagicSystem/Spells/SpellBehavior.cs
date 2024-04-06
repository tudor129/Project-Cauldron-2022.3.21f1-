using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
public class SpellBehavior : BaseSpellBehavior
{
    float _timer;
    float _enemyCheckCounter;
    bool _isInitialized;
    
    Vector3 _originalColliderSize;
    Vector3 _originalColliderCenter;
    Vector3 _lastPosition;
    Vector3 _movement;

    public float speed = 5f;
     Vector3 _randomDirection;

    protected override void Awake()
    {
        base.Awake();
        
        _part = GetComponent<ParticleSystem>();
        _collider = GetComponentInChildren<BoxCollider>();
        
    }

    protected override void Start()
    {
        UpdateParticleSystemValues(_part, _currentStats.Lifetime);
        
        _originalColliderSize = _collider.size;
        _originalColliderCenter = _collider.center;
        
        _lastPosition = transform.position;

        _randomDirection = Random.insideUnitSphere;
        
    }
    
    public void Initialize(Spell spell)
    {
        if (_isInitialized) return;  // Prevent re-initialization

        
        this.spell = spell;
        _currentStats = spell.GetStats();
        _isInitialized = true;

        

        // Perform any operations previously in Awake or Start that depend on initialization here
        PostInitialization();
    }

    void PostInitialization()
    {
        
    }
    
    void UpdateParticleSystemValues(ParticleSystem particleSystem, float newValue)
    {
        // Stop the particle system with clearing all particles
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Update all the needed values here
        var mainModule = particleSystem.main;
        mainModule.duration = newValue;

        // Restart the particle system
        particleSystem.Play();
    }

    protected void Update()
    {
        _randomDirection.y = 0f;
        _randomDirection = _randomDirection.normalized;
        transform.position += _randomDirection * speed * Time.deltaTime;
        
        _movement = transform.position - _lastPosition;
        
        
        if (_movement.x > 0 && _movement.z > 0)
        {
            transform.rotation = Quaternion.Euler(0, 45, 0);
            
            _collider.size += new Vector3(0, (_movement.y), Mathf.Abs(_movement.z));
        
            _collider.center -= new Vector3(0, _movement.y, _movement.z / 2);
            
            _movement = Vector3.zero;
        }
        else if (_movement.x < 0 && _movement.z > 0)
        {
            transform.rotation = Quaternion.Euler(0, -45, 0);
            
            _collider.size += new Vector3(0, (_movement.y), Mathf.Abs(_movement.z));
        
            _collider.center -= new Vector3(0, _movement.y, _movement.z / 2);
            
            _movement = Vector3.zero;
        }
        else if (_movement.z < 0 && _movement.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, -45, 0);
            
            _collider.size += new Vector3(0, (_movement.y), Mathf.Abs(_movement.z));
        
            _collider.center -= new Vector3(0, _movement.y, _movement.z / 2);
            
            _movement = Vector3.zero;
        }
        else if (_movement.z < 0 && _movement.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 45, 0);
            
            _collider.size += new Vector3(0, (_movement.y), Mathf.Abs(_movement.z));
        
            _collider.center -= new Vector3(0, _movement.y, _movement.z / 2);
            
            _movement = Vector3.zero;
        }
        else if (_movement.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            
            _collider.size += new Vector3(Mathf.Abs(_movement.x), (_movement.y), Mathf.Abs(0));
            
            _collider.center += new Vector3(-_movement.x / 2, _movement.y, -_movement.z / 2);
            
            _movement = Vector3.zero;
        }
        else if (_movement.x < 0)
        {
             transform.rotation = Quaternion.Euler(0, 0, 0);
            
            _collider.size += new Vector3(Mathf.Abs(_movement.x), (_movement.y), Mathf.Abs(0));
        
            _collider.center -= new Vector3(_movement.x / 2, _movement.y, _movement.z / 2);
            
            _movement = Vector3.zero;
        }
        else if (_movement.z > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            
            _collider.size += new Vector3(Mathf.Abs(_movement.x), (_movement.y), Mathf.Abs(_movement.z));
        
            // Update collider center - Add half the movement to the center to keep it centered around the object's path
            _collider.center += new Vector3(-_movement.x / 2, _movement.y, -_movement.z / 2);
            
            _movement = Vector3.zero;
        }
        else if (_movement.z < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            
            _collider.size += new Vector3(Mathf.Abs(_movement.x), (_movement.y), Mathf.Abs(_movement.z));
        
            _collider.center -= new Vector3(_movement.x / 2, _movement.y, _movement.z / 2);
            
            _movement = Vector3.zero;
        }
        
        else if (_movement == Vector3.zero)
        {
            _collider.size = _originalColliderSize;
            _collider.center = _originalColliderCenter;
        }
       
        
        _lastPosition = transform.position;
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Get the enemy's health component.
            Health enemyHealth = other.GetComponent<Health>();
            // If the enemy has a health component, deal damage.
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
            }
        }
    }

    Vector3 FindNearestEnemyPosition()
    {
        float minDistance = float.MaxValue;
        Vector3 nearestEnemyPosition = transform.position;
        bool enemyFound = false;

        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _currentStats.SpellRadius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(_player.transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemyPosition = enemy.transform.position;
                    enemyFound = true;
                }
            }
        }

        // Return the position of the nearest enemy or a random position around the player if no enemy is found
        return enemyFound ? nearestEnemyPosition : GetRandomPositionAroundPlayer();
    }
    
    Vector3 GetRandomPositionAroundPlayer()
    {
        Vector3 targetPosition;
        // Use random wandering behavior
        Vector3 randomDirection = Random.insideUnitCircle.normalized * _currentStats.SpellRadius;
        targetPosition = _player.transform.position + new Vector3(randomDirection.x, 10, randomDirection.z);
        
        return targetPosition;
    }
    
    IEnumerator MoveToTargetPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        float moveTime = 1.0f; // Adjust this for the speed of the movement

        Vector3 startingPosition = transform.position;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure the object reaches the exact target position
        transform.position = targetPosition;
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;
public class SpellBehavior : BaseSpellBehavior
{
    float _timer;
    float _enemyCheckCounter;


    public bool _hasMovement;
    public float speed = 3f;
    Vector3 _randomDirection;
    
    SphereCollider _sphereCollider;
    

    protected override void Awake()
    {
        base.Awake();
        
        _part = GetComponent<ParticleSystem>();
        _collider = GetComponentInChildren<BoxCollider>();
        
    }

    protected override void Start()
    {
        UpdateParticleSystemValues(_part, _currentStats.Lifetime);
        
        _randomDirection = Random.insideUnitSphere;
        
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
        if (_hasMovement)
        {
            _randomDirection.y = 0f;
            _randomDirection = _randomDirection.normalized;
            transform.position += _randomDirection * speed * Time.deltaTime;
        }
        
        
      
    }

    void OnParticleCollision(GameObject other)
    {
       Debug.Log("Particle collided with: " + other.name);
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

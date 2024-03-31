using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
public class SpellBehavior : BaseSpellBehavior
{
    float _timer;
    float _enemyCheckCounter;
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        _timer -= Time.deltaTime;
        _enemyCheckCounter -= Time.deltaTime;

        if (_timer <= 0)
        {
            Vector3 targetPosition;

            if (_enemyCheckCounter <= 0)
            {
                targetPosition = FindNearestEnemyPosition();
                _enemyCheckCounter = _currentStats.EnemyCheckTimer;
            }
            else
            {
                // Use random wandering behavior
                Vector3 randomDirection = Random.insideUnitCircle.normalized * _currentStats.WanderRadius;
                targetPosition = _player.transform.position + new Vector3(randomDirection.x, transform.position.y, randomDirection.y);
            }

            StartCoroutine(MoveToTargetPosition(targetPosition));
            _timer = _currentStats.WanderTimer;
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
        targetPosition = _player.transform.position + new Vector3(randomDirection.x, transform.position.y, randomDirection.y);
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTesting : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void  Update()
    {
        if (_playerTransform == null) 
        {
            _playerTransform = GameManager.Instance.GetPlayerTransform();
            if (_playerTransform == null)
            {
                Debug.LogError("Player transform is null!");
                return;
            }
        }
        if (!_health.IsDead())
        {
            //_animator.PlayWalkingAnimation();
            // HandleMovement(); <- we've removed this line
        }
        else if (!_returningToPool)
        {
            // Return to pool
            //_animator.StopWalkingAnimation();
            //_returningToPool = true;
            //StartCoroutine(DelayedReturn(gameObject, 5f));
        }
    }
}

    


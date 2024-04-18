using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTornadoBehavior : SpellBehavior
{
    public float changeDirectionInterval = 5f; // Time in seconds between direction changes
    public float maxDirectionChange = 180.0f; // Max degrees the tornado can turn per interval
    private float timeSinceLastChange = 0.0f;
    Vector3 _targetDirection;
  
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        _targetDirection = _randomDirection;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (_hasMovement)
        {
            _randomDirection.y = 0f;
            _randomDirection = _randomDirection.normalized;
            transform.position += _randomDirection * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            
            /*// Update the timer
            timeSinceLastChange += Time.deltaTime;

            // When it's time to change direction, set a new target direction
            if (timeSinceLastChange >= changeDirectionInterval)
            {
                timeSinceLastChange = 0f;
                _targetDirection = Quaternion.Euler(0, Random.Range(-maxDirectionChange, maxDirectionChange), 0) * _targetDirection;
            }

            // Smoothly interpolate to the new direction
            _randomDirection = Vector3.RotateTowards(_randomDirection, _targetDirection, maxDirectionChange * Mathf.Deg2Rad * Time.deltaTime, 0f);
            _randomDirection.y = 0; // Keep the direction horizontal

            // Move the tornado in the current direction
            transform.position += _randomDirection * speed * Time.deltaTime;

            // Correct the rotation of the tornado
            transform.rotation = Quaternion.LookRotation(_randomDirection) * Quaternion.Euler(-90, 0, 0);*/
           
        }
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerLocomotionManager : MonoBehaviour
{
    [SerializeField] OrbitalMovement _orbitalMovement;
    
    [Header("Movement Variables")]
    [SerializeField] public float _moveSpeed = 8f;
    [SerializeField] float _gravity = -30f;
    [SerializeField] float _rotationSpeed = 15f;
    
    Player _player;
    CharacterController _characterController;
    PlayerAnimatorManager _animatorManager;
    
    Vector2 _inputVector;
    Vector3 _velocity;
    Vector3 _currentVelocity;
    [FormerlySerializedAs("_rollDirection")] public Vector3 _dashDirection;
    public Vector3 _moveDir;
   
    
     void OnEnable()
    {
        InputManager.Instance.PlayerInputActions.Player.Movement.performed += Movement_performed;
        InputManager.Instance.PlayerInputActions.Player.Movement.canceled += Movement_cancelled;
        InputManager.Instance.PlayerInputActions.Player.Aiming.performed += Aiming_performed;
        InputManager.Instance.PlayerInputActions.Player.Dodge.performed += Dodge_performed;
        
    }
    void OnDisable()
    {
        InputManager.Instance.PlayerInputActions.Player.Movement.performed -= Movement_performed;
        InputManager.Instance.PlayerInputActions.Player.Movement.canceled -= Movement_cancelled;
        InputManager.Instance.PlayerInputActions.Player.Aiming.performed -= Aiming_performed;
        InputManager.Instance.PlayerInputActions.Player.Dodge.performed -= Dodge_performed;
    }

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animatorManager = GetComponent<PlayerAnimatorManager>();
        _player = GetComponent<Player>();
    }

    void Start()
    {
        

    }

    void Update()
    {
        
    }

    void Movement_performed(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }
    void Movement_cancelled(InputAction.CallbackContext context)
    {
        _inputVector = Vector2.zero;
    }
    void Aiming_performed(InputAction.CallbackContext context)
    {
        Vector2 aimVector = context.ReadValue<Vector2>();

        if (context.control.device is Mouse)
        {
            Vector2 mousePosition = InputManager.Instance.PlayerInputActions.Player.Aiming.ReadValue<Vector2>();
            RotateToFaceMouse(mousePosition);
        }
        else
        {
            if (aimVector.magnitude > 0.1f)  // Replace 0.1 with whatever threshold suits your game
            {
                Vector3 adjustedAimVector3D = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0) * aimVector;
                HandleGamepadRotation(transform.position + adjustedAimVector3D);
            }
        }
    }
    
    
    public void HandleAllMovement()
    {
        _moveDir = new Vector3(_inputVector.x, 0f, _inputVector.y);
        
        if (_moveDir.magnitude > 1.0f)
        {
            _moveDir.Normalize();
        }
        
        float moveDistance = _moveSpeed * Time.deltaTime;

        // Apply gravity
        if (_characterController.isGrounded)
        {
            _velocity.y = 0f;
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
        }
        _characterController.Move(_velocity * Time.deltaTime);

        float velocityZ = Vector3.Dot(_moveDir.normalized, transform.forward);
        float velocityX = Vector3.Dot(_moveDir.normalized, transform.right);
        
        if (_moveDir != Vector3.zero)
        {
            _animatorManager.UpdateAnimatorMovementParameters(velocityZ, velocityX);
                
            HandleMovement(_moveDir, moveDistance);
        }
        else
        {
            _animatorManager.UpdateAnimatorMovementParameters(0f, 0f);
        }
        
    }
    
    void Dodge_performed(InputAction.CallbackContext context)
    {
        //AttemptToPerformDodge();
    }
    void AttemptToPerformDodge()
    {
        if (_player._isPerformingAction)
        {
            return;
        }

        if (_moveSpeed > 0)
        {
            _dashDirection.y = 0f;
            _dashDirection.Normalize();
            _dashDirection = _moveDir;
            
            Quaternion rollRotation = Quaternion.LookRotation(_dashDirection);
            _player.transform.rotation = rollRotation;
            // PERFORM A ROLL FORWARD ANIMATION
            _animatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true, false, false);
            _player._isDashing = true;
        }
        else
        {
            // PERFORM A BACK STEP ANIMATION
            _animatorManager.PlayTargetActionAnimation("Back_Step_01", true, true, false, false);
        }
    }
    void HandleMovement(Vector3 moveDir, float moveDistance)
    {
        if (_player._isDashing)
        {
            return;
        }
        _characterController.Move(moveDir * moveDistance);
    }
    void RotateToFaceMouse(Vector2 mousePosition)
    {
        if (_player._isDashing)
        {
            return;
        }
        // Convert mouse position to world space
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);  // The plane is at player's height
        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);

            // Get direction from player to mouse
            Vector3 directionToMouse = pointToLook - transform.position;
            directionToMouse.y = 0;
            float rotationSpeed = 15f;
            // Rotate player to face mouse
            Quaternion targetRotation = Quaternion.LookRotation(directionToMouse, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    void HandleGamepadRotation(Vector3 targetPosition)
    {
        // Calculate direction vector
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;  // Ensure it's only in the horizontal plane
        
        // Create target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        
        // Smoothly interpolate rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
   
    
}

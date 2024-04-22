using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class DashAbility : MonoBehaviour
{
    [SerializeField] MMF_Player _feedback;
    
    [FormerlySerializedAs("AbilitySO")] [FormerlySerializedAs("Ability")] public AbilityData abilityData;
    
    [SerializeField] GameObject _firePrefab;
    
    Player _player;
    PlayerLocomotionManager _playerLocomotionManager; 
    CharacterController _characterController;
    PlayerAnimatorManager _animatorManager;
    
    int _numberOfCharges = 10;
    
    void Awake()
    {
        _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        _player = GetComponent<Player>();
        _characterController = GetComponent<CharacterController>();
        _animatorManager = GetComponent<PlayerAnimatorManager>();
    }
    void Start()
    {
        
    }

    void OnEnable()
    {
        InputManager.Instance.PlayerInputActions.Player.Dodge.performed += AttemptToDash;
    }
    void OnDisable()
    {
        InputManager.Instance.PlayerInputActions.Player.Dodge.performed -= AttemptToDash;
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        
    }
    
    void AttemptToDash(InputAction.CallbackContext context)
    {
        
        PerformDash();
    }

    void PerformDash()
    {
        if (_player._isPerformingAction)
        {
            return;
        }
        
        if (_playerLocomotionManager._moveDir == Vector3.zero)
        {
            return;
        }
        
        string chargeToUse = GetNextAvailableCharge();

        if (!string.IsNullOrEmpty(chargeToUse))
        {
            if (_playerLocomotionManager._moveSpeed > 0)
            {
                StartIFrames();
                StopRotation();
                _playerLocomotionManager._dashDirection.y = 0f;
                _playerLocomotionManager._dashDirection.Normalize();
                _playerLocomotionManager._dashDirection = _playerLocomotionManager._moveDir;
                
                Quaternion rollRotation = Quaternion.LookRotation(_playerLocomotionManager._dashDirection);
                // To rotate player towards the direction of the dash
                //_player.transform.rotation = rollRotation;
                
                StartCoroutine(Dash());
                
                GameObject dashObject = Instantiate(abilityData.AbilityEffectsPrefab, transform.position + Vector3.up, Quaternion.identity);
                GameObject dashStartObject = Instantiate(abilityData.AbilityStartEffect, transform.position + Vector3.up, Quaternion.identity);
                MeshTrail meshTrail = gameObject.AddComponent<MeshTrail>();
                meshTrail.abilityData = abilityData;

                //_feedback.PlayFeedbacks();
                
                CooldownManager.Instance.StartCooldown(chargeToUse, abilityData.Cooldown);
                _player._isPerformingAction = false;
                Destroy(dashStartObject, abilityData.Duration + 1f);
            }
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        float totalDistance = 0f;

        while (Time.time < startTime + abilityData.Duration && totalDistance < abilityData.Range)
        {
            Vector3 dashStep = _playerLocomotionManager._dashDirection * abilityData.Speed * Time.deltaTime;
            _characterController.Move(dashStep);
            totalDistance += dashStep.magnitude; // Update the total distance traveled
            yield return null;
        }
    }
    
    string GetNextAvailableCharge()
    {
        for (int i = 1; i <= abilityData.NumberOfCharges; i++)
        {
            string chargeName = $"{abilityData.ItemName}_{i}";
            if (!CooldownManager.Instance.IsOnCooldown(chargeName))
            {
                _player._isPerformingAction = true;
                return chargeName; // Return the first available charge
            }
        }
        return null; // No charges available
    }
    
    void StartIFrames()
    {
        _player._isInvulnerable = true;
        Invoke("EndIFrames", abilityData.IFramesTime);
    }
    void EndIFrames()
    {
        _player._isInvulnerable = false;
    }

    void StopRotation()
    {
        _player._isDashing = true;
        Invoke("StartRotation", abilityData.Duration);
    }
    void StartRotation()
    {
        _player._isDashing = false;
    }
}

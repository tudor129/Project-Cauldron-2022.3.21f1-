using DG.Tweening;
using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class Player : MonoBehaviour, IObjectParent
{
    public static Player Instance { get; private set; }

    [SerializeField] Transform _objectHolder;
    [SerializeField] Cauldron _cauldron;
    [SerializeField] [CanBeNull] MMFeedbacks _feedbacks;
    
    [Header("Flags")]
    public bool _isPerformingAction = false;
    public bool _applyRootMotion = false;
    public bool _canRotate = true;
    public bool _canMove = true;
    [FormerlySerializedAs("_isRolling")] public bool _isDashing = false;
    public bool _isInvulnerable = false;


    [HideInInspector] public CharacterController _characterController;
    PlayerLocomotionManager _locomotionManager;
 
    DeathDetector _deathDetector;
    Animator _animator;

    Vector3 _currentVelocity;
    Vector3 _velocity;
    Vector2 _inputVector;
    
    bool _isWalking;
    bool _isAttacking;
    float _attackCooldown = 0.8f;
    float _timeSinceLastAttack = 0.0f;
    
    void OnEnable()
    {
        InputManager.Instance.PlayerInputActions.Player.SpawnFromCauldron.performed += SpawnFromCauldron_performed;
        InputManager.Instance.PlayerInputActions.Player.DropItem.performed += DropItem_performed;
        InputManager.Instance.PlayerInputActions.Player.ResetCauldron.performed += ResetCauldron_performed;
        InputManager.Instance.PlayerInputActions.Player.Attack.performed += Attack_performed;
        InputManager.Instance.PlayerInputActions.Player.Attack.canceled += Attack_cancelled;
        InputManager.Instance.PlayerInputActions.Player.Revive.performed += Revive_performed;

    }
    void OnDisable()
    {
        InputManager.Instance.PlayerInputActions.Player.SpawnFromCauldron.performed -= SpawnFromCauldron_performed;
        InputManager.Instance.PlayerInputActions.Player.DropItem.performed -= DropItem_performed;
        InputManager.Instance.PlayerInputActions.Player.ResetCauldron.performed -= ResetCauldron_performed;
        InputManager.Instance.PlayerInputActions.Player.Attack.performed -= Attack_performed;
        InputManager.Instance.PlayerInputActions.Player.Attack.canceled -= Attack_cancelled;
        InputManager.Instance.PlayerInputActions.Player.Revive.performed -= Revive_performed;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one Player instance!" + gameObject.name + " is being destroyed!");
            Destroy(gameObject);
        }
        
        Instance = this;

        if (this.tag != "Player")
        {
            this.tag = "Player";
            Debug.LogWarning("Player tag was not set! Auto-correcting...");
        }
        
        GameManager.Instance.RegisterPlayer(transform);
        
        _deathDetector = GetComponent<DeathDetector>();
        _locomotionManager = GetComponent<PlayerLocomotionManager>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        
    }

    // MOVEMENT WILL BE HANDLED IN THE PLAYER LOCOMOTION MANAGER CLASS
    void Update()
    {
        //HandleFlags();
        _locomotionManager.HandleAllMovement();
    }

    void FixedUpdate()
    {
        _timeSinceLastAttack += Time.fixedDeltaTime; // Increment the time since the last attack

        if (_isAttacking && _timeSinceLastAttack >= _attackCooldown)
        {
            // THIS WILL BE THE MAIN ATTACK FUNCTION
            //_playerAttack.PerformAttack();
            _timeSinceLastAttack = 0.0f; // Reset the time since the last attack
        }
    }

    void HandleFlags()
    {
        if (_applyRootMotion == true)
        {
            _animator.applyRootMotion = true;
        }
        else
        {
            _animator.applyRootMotion = false;
        }
    }

    // THIS WILL LATER BE IN IT'S OWN ACTIONS CLASS
    #region Actions
    
    void Revive_performed(InputAction.CallbackContext context)
    {
        GameObject deadEnemy = _deathDetector.GetDeadEnemy();
        if (deadEnemy != null)
        {
            deadEnemy.GetComponent<Health>().Respawn();
        }
    }
    void Attack_performed(InputAction.CallbackContext context)
    {
        _isAttacking = true;
    }
    void Attack_cancelled(InputAction.CallbackContext context)
    {
        _isAttacking = false;
    }
  
    void SpawnFromCauldron_performed(InputAction.CallbackContext context)
    {
        HandleSpawnLastRecipe();
    }
    void DropItem_performed(InputAction.CallbackContext context)
    {
        
    }
    void ResetCauldron_performed(InputAction.CallbackContext context)
    {
        //_cauldron.ResetCauldron();
    }
  
    #endregion
    
   
  
    void HandleSpawnLastRecipe()
    {
        //_cauldron.CheckCraftTimeValidity();
        //_cauldron.SpawnLastMatchedRecipePickup(_objectHolder); 
    }
 
    
   
    public GameObject IsCloseToObject(GameObject[] potions, float proximityThreshold)
    {
        GameObject closestItem = null;
        float closestDistance = proximityThreshold;
        foreach (GameObject potion in potions)
        {
            float distanceToItem = Vector3.Distance(this.transform.position, potion.transform.position);

            if (distanceToItem < closestDistance)
            {
                closestItem = potion;
                closestDistance = distanceToItem;
            }
        }

        return closestItem;
    }
    List<CurrencyPickup> GetPickupsInRadius(float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("CurrencyPickup"));

        List<CurrencyPickup> pickups = new List<CurrencyPickup>();
        foreach (var hitCollider in hitColliders)
        {
            CurrencyPickup pickup = hitCollider.GetComponent<CurrencyPickup>();
            if (pickup != null)
            {
                pickups.Add(pickup);
            }
        }

        return pickups;
    }

    void HandleMagneticPickup()
    {
        List<CurrencyPickup> pickups = GetPickupsInRadius(5f);
        foreach (CurrencyPickup pickup in pickups)
        {
            // handle pickup, for example:
            switch (pickup.currencyType)
            {
                case CurrencyPickup.CurrencyType.Coin:
                    CurrencyManager.Instance.AddCoins(1);
                    break;
                case CurrencyPickup.CurrencyType.Gem:
                    CurrencyManager.Instance.AddGems(1);
                    ExperienceManager.Instance.AddExperience(10);
                    break;
            }
            
            Destroy(pickup.gameObject);
        }
    }
    
    public Transform GetObjectHolder()
    {
        return _objectHolder;
    }
   
}
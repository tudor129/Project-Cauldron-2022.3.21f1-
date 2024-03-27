using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    PlayerInputActions _playerInputActions;
    InputAction _click;
    
    public PlayerInputActions PlayerInputActions
    {
        get
        {
            return _playerInputActions;
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("More than one instance of InputManager in the scene!");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        _playerInputActions = new PlayerInputActions();
    }
    
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined; 
        Cursor.visible = true;
    }

    void OnEnable()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.Spells.Enable();

        _playerInputActions.Spells.ActiveSpell.performed += OnActiveSpellCast;
        _playerInputActions.Spells.Spell1.performed += OnSpellSlot1;
        _playerInputActions.Spells.Spell2.performed += OnSpellSlot2;
        _playerInputActions.Spells.Spell3.performed += OnSpellSlot3;
        _playerInputActions.Spells.Spell4.performed += OnSpellSlot4;
    }
   
    void OnDisable()
    {
        _playerInputActions.Spells.Disable();
        _playerInputActions.Player.Disable();
        
        _playerInputActions.Spells.ActiveSpell.performed += OnActiveSpellCast;
        _playerInputActions.Spells.Spell1.performed += OnSpellSlot1;
        _playerInputActions.Spells.Spell2.performed += OnSpellSlot2;
        _playerInputActions.Spells.Spell3.performed += OnSpellSlot3;
        _playerInputActions.Spells.Spell4.performed += OnSpellSlot4;
    }
    
    void OnActiveSpellCast(InputAction.CallbackContext obj)
    {
        //SpellManager.Instance.CastActiveSpell();
    }
    // These are for testing spells
    void OnSpellSlot4(InputAction.CallbackContext obj)
    {
        //SpellManager.Instance.OnSpellSlot1();
    }
    void OnSpellSlot3(InputAction.CallbackContext obj)
    {
        //SpellManager.Instance.OnSpellSlot2();
    }
    void OnSpellSlot2(InputAction.CallbackContext obj)
    {
        //SpellManager.Instance.OnSpellSlot3();
    }
    void OnSpellSlot1(InputAction.CallbackContext obj)
    {
        //SpellManager.Instance.OnSpellSlot4();
    }
    
    
    
}
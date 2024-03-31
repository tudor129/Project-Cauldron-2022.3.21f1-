using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.VFX;
using Image = UnityEngine.UI.Image;



public class SpellManager : MonoBehaviour, ICast
{
    
    [System.Serializable]
    public class Slot
    {
        public Item item;
        public Image image;

        public void Assign(Item assignedItem)
        {
            item = assignedItem;
            if(item is Spell)
            {
                Spell w = item as Spell;
                // image.enabled = true;
                // image.sprite = w.spellData.icon;
            }
            else
            {
                Potion p = item as Potion;
                // image.enabled = true;
                // image.sprite = p.potionData.icon;
            }
            Debug.Log(string.Format("Assigned {0} to player.", item.name));
        }

        public void Clear()
        {
            item = null;
            image.enabled = false;
            image.sprite = null;
        }

        public bool IsEmpty()
        {
            return item == null;
        }
    }
    
    public List<Slot> _spellSlots = new List<Slot>();
    public List<Slot> _potionSlots = new List<Slot>();
    
    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text UpgradeNameDisplay;
        public TMP_Text UpgradeDescriptionDisplay;
        public Image UpgradeIcon;
        public Button UpgradeButton;
    }
    
    [Header("UI Elements")]
    public List<SpellData> _availableSpells = new List<SpellData>();    //List of upgrade options for spells
    public List<PotionData> _availablePotions = new List<PotionData>(); //List of upgrade options for potions 
    public List<UpgradeUI> _upgradeUIOptions = new List<UpgradeUI>();    //List of ui for upgrade options present in the scene
    
    PlayerStats _playerStats;
    [SerializeField] GameObject _levelUpScreen;

    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // Checks if the inventory has an item of a certaint type.
    public bool Has(ItemData type) { return Get(type); }

    public Item Get(ItemData type)
    {
        if (type is SpellData) return Get(type as SpellData);
        else if (type is PotionData) return Get(type as PotionData);
        return null;
    }
    
    // Find a potion of a certain type in the inventory.
    public Potion Get(PotionData type)
    {
        foreach (Slot s in _potionSlots)
        {
            Potion p = s.item as Potion;
            if (p.potionData == type)
                return p;
        }
        return null;
    }
    
    // Find a spell of a certain type in the inventory.
    public Spell Get(SpellData type)
    {
        foreach (Slot s in _spellSlots)
        {
            Spell w = s.item as Spell;
            if (w.spellData == type)
                return w;
        }
        return null;
    }
    
    // Removes a spell of a particular type, as specified by <data>.
    public bool Remove(SpellData data, bool removeUpgradeAvailability = false)
    {
        // Remove this weapon from the upgrade pool.
        if (removeUpgradeAvailability) _availableSpells.Remove(data);

        for(int i = 0; i < _spellSlots.Count; i++)
        {
            Spell w = _spellSlots[i].item as Spell;
            if (w.spellData == data)
            {
                _spellSlots[i].Clear();
                w.OnUnequip();
                Destroy(w.gameObject);
                return true;
            }
        }

        return false;
    }
    
    // Removes a passive of a particular type, as specified by <data>.
    public bool Remove(PotionData data, bool removeUpgradeAvailability = false)
    {
        // Remove this passive from the upgrade pool.
        if (removeUpgradeAvailability) _availablePotions.Remove(data);

        for (int i = 0; i < _potionSlots.Count; i++)
        {
            Potion p = _potionSlots[i].item as Potion;
            if (p.potionData == data)
            {
                _potionSlots[i].Clear();
                p.OnUnequip();
                Destroy(p.gameObject);
                return true;
            }
        }

        return false;
    }
    
    // If an ItemData is passed, determine what type it is and call the respective overload.
    // We also have an optional boolean to remove this item from the upgrade list.
    public bool Remove(ItemData data, bool removeUpgradeAvailability = false)
    {
        if (data is PotionData) return Remove(data as PotionData, removeUpgradeAvailability);
        else if(data is SpellData) return Remove(data as SpellData, removeUpgradeAvailability);
        return false;
    }
    
    // Finds an empty slot and adds a weapon of a certain type, returns
    // the slot number that the item was put in.
    public int Add(SpellData data)
    {
        int slotNum = -1;

        // Try to find an empty slot.
        for(int i = 0; i < _spellSlots.Capacity; i++)
        {
            if (_spellSlots[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        // If there is no empty slot, exit.
        if (slotNum < 0) return slotNum;

        // Otherwise create the weapon in the slot.
        // Get the type of the weapon we want to spawn.
        Type spellType = Type.GetType(data.Behaviour);

        if (spellType != null)
        {
            // Spawn the weapon GameObject.
            GameObject go = new GameObject(data.BaseStats.Name + " Controller");
            Spell spawnedSpell = (Spell)go.AddComponent(spellType);
            spawnedSpell.Initialise(data);
            spawnedSpell.InitializeData(data);
            spawnedSpell.transform.SetParent(transform); //Set the weapon to be a child of the player
            spawnedSpell.transform.localPosition = Vector2.zero;
            spawnedSpell.OnEquip();

            // Assign the weapon to the slot.
            _spellSlots[slotNum].Assign(spawnedSpell);
            
            // // Close the level up UI if it is on.
            // // This will be added later
            // if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            //     GameManager.instance.EndLevelUp();
            
            _levelUpScreen.gameObject.SetActive(false);
            
            return slotNum;
        }
        else
        {
            Debug.LogWarning(string.Format(
                "Invalid spell type specified for {0}.",
                data.name
            ));
        }

        return -1;
    }
    
    public int Add(PotionData data)
    {
        int slotNum = -1;

        // Try to find an empty slot.
        for (int i = 0; i < _potionSlots.Capacity; i++)
        {
            if (_potionSlots[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        // If there is no empty slot, exit.
        if (slotNum < 0) return slotNum;

        // Otherwise create the passive in the slot.
        // Get the type of the passive we want to spawn.
        GameObject go = new GameObject(data.baseStats.name + " Potion");
        Potion p = go.AddComponent<Potion>();
        p.Initialise(data);
        p.transform.SetParent(transform); //Set the weapon to be a child of the player
        p.transform.localPosition = Vector2.zero;

        // Assign the passive to the slot.
        _potionSlots[slotNum].Assign(p);

        // if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        // {
        //     GameManager.instance.EndLevelUp();
        // }
        // player.RecalculateStats();

        return slotNum;
    }

    // If we don't know what item is being added, this function will determine that.
    public int Add(ItemData data)
    {
        if (data is SpellData) return Add(data as SpellData);
        else if (data is PotionData) return Add(data as PotionData);
        return -1;
    }
    
    public void LevelUpSpell(int slotIndex, int upgradeIndex)
    {
        if (_spellSlots.Count > slotIndex)
        {
            Spell spell = _spellSlots[slotIndex].item as Spell;

            // Don't level up the weapon if it is already at max level.
            if (!spell.LevelUp())
            {
                Debug.LogWarning(string.Format(
                    "Failed to level up {0}.",
                    spell.name
                ));
                return;
            }
        }
        
        _levelUpScreen.gameObject.SetActive(false);
        // if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        // {
        //     GameManager.instance.EndLevelUp();
        // }
    }
    
    public void LevelUpPotion(int slotIndex, int upgradeIndex)
    {
        /*if (_potionSlots.Count > slotIndex)
        {
            Potion p = _potionSlots[slotIndex].item as Potion;
            if(!p.DoLevelUp())
            {
                Debug.LogWarning(string.Format(
                    "Failed to level up {0}.",
                    p.name
                ));
                return;
            }
        }

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
        player.RecalculateStats();*/
    }
    
    void ApplyUpgradeOptions()
    {
        // Make a duplicate of the available spell / potion upgrade lists
        // so we can iterate through them in the function.
        List<SpellData> availableWeaponUpgrades = new List<SpellData>(_availableSpells);
        List<PotionData> availablePassiveItemUpgrades = new List<PotionData>(_availablePotions);

        // Iterate through each slot in the upgrade UI.
        foreach (UpgradeUI upgradeOption in _upgradeUIOptions)
        {
            // If there are no more avaiable upgrades, then we abort.
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
                return;

            // Determine whether this upgrade should be for passive or active spells.
            int upgradeType;
            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                // Random generates a number between 1 and 2.
                upgradeType = UnityEngine.Random.Range(1, 3);
            }

            // Generates an active weapon upgrade.
            if (upgradeType == 1)
            {
                
                // Pick a spell upgrade, then remove it so that we don't get it twice.
                SpellData chosenSpellUpgrade = availableWeaponUpgrades[UnityEngine.Random.Range(0, availableWeaponUpgrades.Count)];
                availableWeaponUpgrades.Remove(chosenSpellUpgrade);

                // Ensure that the selected weapon data is valid.
                if (chosenSpellUpgrade != null)
                {
                    // Turns on the UI slot.
                    EnableUpgradeUI(upgradeOption);

                    // Loops through all our existing spells. If we find a match, we will
                    // hook an event listener to the button that will level up the spell
                    // when this upgrade option is clicked.
                    bool isLevelUp = false;
                    for (int i = 0; i < _spellSlots.Count; i++)
                    {
                        Spell w = _spellSlots[i].item as Spell;
                        if (w != null && w.spellData == chosenSpellUpgrade)
                        {
                            // If the weapon is already at the max level, do not allow upgrade.
                            if (chosenSpellUpgrade.MaxLevel <= w.CurrentLevel)
                            {
                                DisableUpgradeUI(upgradeOption);
                                isLevelUp = true;
                                break;
                            }

                            // Set the Event Listener, item and level description to be that of the next level
                            upgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpSpell(i, i)); //Apply button functionality
                            Spell.Stats nextLevel = chosenSpellUpgrade.GetLevelData(w.CurrentLevel + 1);
                            upgradeOption.UpgradeDescriptionDisplay.text = nextLevel.Description;
                            upgradeOption.UpgradeNameDisplay.text = nextLevel.Name;
                            upgradeOption.UpgradeIcon.sprite = chosenSpellUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    // If the code gets here, it means that we will be adding a new spell, instead of
                    // upgrading an existing spell.
                    if (!isLevelUp)
                    {
                        upgradeOption.UpgradeButton.onClick.AddListener(() => Add(chosenSpellUpgrade)); //Apply button functionality
                        upgradeOption.UpgradeDescriptionDisplay.text = chosenSpellUpgrade.BaseStats.Description;  //Apply initial description
                        upgradeOption.UpgradeNameDisplay.text = chosenSpellUpgrade.BaseStats.Name;    //Apply initial name
                        upgradeOption.UpgradeIcon.sprite = chosenSpellUpgrade.icon;
                    }
                }
            }
            else if (upgradeType == 2)
            {
                // NOTE: We have to recode this system, as right now it disables an upgrade slot if
                // we hit a weapon that has already reached max level.
                PotionData chosenPassiveUpgrade = availablePassiveItemUpgrades[UnityEngine.Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveUpgrade);

                if (chosenPassiveUpgrade != null)
                {
                    // Turns on the UI slot.
                    EnableUpgradeUI(upgradeOption);

                    // Loops through all our existing passive. If we find a match, we will
                    // hook an event listener to the button that will level up the weapon
                    // when this upgrade option is clicked.
                    bool isLevelUp = false;
                    for (int i = 0; i < _potionSlots.Count; i++)
                    {
                        Potion p = _potionSlots[i].item as Potion;
                        if (p != null && p.potionData == chosenPassiveUpgrade)
                        {
                            // If the passive is already at the max level, do not allow upgrade.
                            if (chosenPassiveUpgrade.MaxLevel <= p.CurrentLevel)
                            {
                                DisableUpgradeUI(upgradeOption);
                                isLevelUp = true;
                                break;
                            }
                            upgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpPotion(i, i)); //Apply button functionality
                            Potion.Modifier nextLevel = chosenPassiveUpgrade.GetLevelData(p.CurrentLevel + 1);
                            upgradeOption.UpgradeDescriptionDisplay.text = nextLevel.description;
                            upgradeOption.UpgradeNameDisplay.text = nextLevel.name;
                            upgradeOption.UpgradeIcon.sprite = chosenPassiveUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    if (!isLevelUp) //Spawn a new potion 
                    {
                        upgradeOption.UpgradeButton.onClick.AddListener(() => Add(chosenPassiveUpgrade)); //Apply button functionality
                        Potion.Modifier nextLevel = chosenPassiveUpgrade.baseStats;
                        upgradeOption.UpgradeDescriptionDisplay.text = nextLevel.description;  //Apply initial description
                        upgradeOption.UpgradeNameDisplay.text = nextLevel.name;  //Apply initial name
                        upgradeOption.UpgradeIcon.sprite = chosenPassiveUpgrade.icon;
                    }
                }
            }
        }
    }
    
    void RemoveUpgradeOptions()
    {
        foreach (UpgradeUI upgradeOption in _upgradeUIOptions)
        {
            upgradeOption.UpgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);    //Call the DisableUpgradeUI method here to disable all UI options before applying upgrades to them
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.UpgradeNameDisplay.transform.parent.gameObject.SetActive(false);
        
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.UpgradeNameDisplay.transform.parent.gameObject.SetActive(true);
        
    }
    
    
    public void Cast(SpellInstance spellInstance)
    {
        throw new NotImplementedException();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /*public static SpellManager Instance { get; private set; }
    
    public SpellSO SpellSO;
    public LightningStrikeSO LightningStrikeSO;
    
    
    // List to store available spells
    [ShowInInspector]
    [SerializeField]
    public List<SpellSO> _availableSpells = new List<SpellSO>();
    
    [FormerlySerializedAs("_aquiredSpells")]
    [ShowInInspector]
    [SerializeField]
    public List<SpellInstance> _acquiredSpells = new List<SpellInstance>();

   
    
    // Dictionary to store active spells
    [ShowInInspector]
    [SerializeField]
    public Dictionary<string, SpellInstance> _activeSpells = new Dictionary<string, SpellInstance>();
    
    
   
    
    [SerializeField] MMFeedbacks _mmFeedbacks;
    [SerializeField, Required] PlayerStatSO _playerStats;
    [SerializeField] Transform _castPoint;
    [SerializeField] Image _spellIcon;
    [SerializeField, Required] Storage _storage;

    ISpell _spell;
    SpellSO _currentSpellSO;
    SpellInstance _currentSpellInstance;
   
    GameObject _currentCastEffect;
    
    Player _player;
    PlayerAnimatorManager _animatorManager;
    
    Vector3 _targetPosition;
    Vector3 _targetRotation;
    
    float _currentManaRechargeTimer;
    float _currentCastTimer;
    bool _castingSpell;

    const string CAST_SPELL_ANIM = "BaseAttack";
    
    Vector3 _lastPlayerPosition;
    [SerializeField] float _distanceTraveled;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError($"Another instance of {nameof(SpellManager)} already exists! Destroying duplicate.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        _player = GetComponent<Player>();
        _animatorManager = GetComponent<PlayerAnimatorManager>();
         _activeSpells.Clear();
    }

    void Start()
    {
        //AddSpellToDictionary(SpellSO);
        //AddNewActiveSpellInstance(SpellSO);
        _lastPlayerPosition = _player.transform.position;
    }

    void Update()
    {
        UpdateDistanceTraveled();
    }
    
    void UpdateDistanceTraveled()
    {
        // Calculate the distance the player has moved since the last frame
        float distanceMoved = Vector3.Distance(_player.transform.position, _lastPlayerPosition);

        // Update the total distance traveled
        _distanceTraveled += distanceMoved;

        // Update lastPosition for the next frame
        _lastPlayerPosition = transform.position;
        
        AutoCastSpell();
       
    }
    
    // These are just for testing spells
    public void OnSpellSlot1()
    {
        CastSpellFromStorage(0); 
    }
    public void OnSpellSlot2()
    {
        CastSpellFromStorage(1); 
    }
    public void OnSpellSlot3()
    {
        CastSpellFromStorage(2); 
    }
    public void OnSpellSlot4()
    {
        CastSpellFromStorage(3); 
    }
   
    public void CastActiveSpell()
    {
        //SpellSO spellData = GetFirstSpellSO();
        //SpellInstance spellInstance = GetSpellInstance(spellData.name);
        //_isLeftMouseButtonHeldDown = true;
        // if (CooldownManager.Instance.IsOnCooldown(spellInstance.Spell.ItemName))
        // {
        //     return;
        // }
        AttemptToCastActiveSpell();
    }
    
    void AttemptToCastActiveSpell()
    { 
       if (_player._isPerformingAction) return; 
       if (_player._isDashing) return;
       _player._isPerformingAction = true;
       _castingSpell = false; 
       if (_castingSpell) return;
       
       //SpellSO spellData = GetFirstSpellSO();
       //SpellInstance spellInstance = GetSpellInstance(spellData.name);
      
       
       if (SpellSO.IsFire)
       {
           if (SpellSO.HasCastEffect)
           {
               //_currentCastEffect =  Instantiate(spellInstance.Spell.SpellCastEffect, _player.transform.position, transform.rotation);
               _currentCastEffect.transform.localScale = new Vector3(0.35f, 0.35f, 1f);
               _currentCastEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);

               //CooldownManager.Instance.StartCooldown(spellInstance.Spell.ItemName, spellInstance.Spell.BaseCooldown);
           }
       } 
       if (SpellSO.IsArcane)
       {
           if (SpellSO.HasCastEffect)
           {
               _currentCastEffect =  Instantiate(SpellSO.SpellCastEffect, _castPoint.transform.position, transform.rotation);
               _currentCastEffect.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
               _currentCastEffect.transform.localRotation = Quaternion.Euler(0, 0, 0);
           }
       }
       if (SpellSO.IsIce)
       {
           if (SpellSO.HasCastEffect)
           {
               _currentCastEffect =  Instantiate(SpellSO.SpellCastEffect, _player.transform.position + Vector3.up, transform.rotation);
               _currentCastEffect.transform.localScale = new Vector3(0.25f, 0.25f, 1.5f);
               _currentCastEffect.transform.localRotation = Quaternion.Euler(-90, 0, 0);
           }
       }

       _animatorManager.PlayTargetActionAnimation(CAST_SPELL_ANIM, true, false, true, true);
    }
    
    
    
    void AutoCastSpell()
    {
        if (_distanceTraveled >= LightningStrikeSO.PlayerTravelDistance)
        {
            // Find or create the SpellInstance for LightningStrikeSO
            //SpellInstance spellInstance = GetOrCreateSpellInstance(LightningStrikeSO);

            //Cast(spellInstance);
            _distanceTraveled = 0;
        }
    }
    void CastSpellFromStorage(int index)
    {
        if (_storage != null)
        {
            Item selectedItem = _storage.GetItem(index);
            if (selectedItem is SpellSO spellSO)
            {
                // Find or create the SpellInstance for the selected spell
                
                //SpellInstance spellInstance = GetOrCreateSpellInstance(spellSO);

                //Cast(spellInstance);
            }
        }
    }
    public void Cast(SpellInstance spellInstance)
    {
        _currentSpellInstance = spellInstance;
        if (spellInstance != null)
        {
            if (!string.IsNullOrEmpty(spellInstance.Spell.SpellAnimation))
            {
                _animatorManager.PlayTargetActionAnimation(spellInstance.Spell.SpellAnimation, true, false, true, true);
            }
            else
            {
                SpellFactory.CreateSpell(spellInstance);
            }
        }
    }
      
    // These methods are called from the animation event
    void SpawnProjectile() 
    {
        _castingSpell = false;
        PerformBaseAttack();
        if (_currentCastEffect != null)
        {
            StopAndDestroyParticleEffect();
            //Destroy(_currentCastEffect, _currentCastEffect.GetComponent<ParticleSystem>().main.duration);
        }
    }
    // ------------------------------------------------------------------------------------------------------
    void PerformBaseAttack()
    {
        _player._isPerformingAction = false;
        
        if ( SpellSO.SpellProjectileFX != null)
        {
            
            //GameObject spell = Instantiate(spellData.SpellProjectileFX, _castPoint.transform.position, transform.rotation);
            
        }
    }
    void StopAndDestroyParticleEffect()
    {
        if (_currentCastEffect != null)
        {
            // Stop the particle system to prevent further emission
            _currentCastEffect.GetComponent<ParticleSystem>().Stop();

            StartCoroutine(DestroyAfterParticleEffect());
        }
    }
    
    IEnumerator DestroyAfterParticleEffect()
    {
        // Wait for the particle system to finish
        while (_currentCastEffect != null && _currentCastEffect.GetComponent<ParticleSystem>().IsAlive(true))
        {
            yield return null;
        }

        // Particle system has finished, destroy the effect
        Destroy(_currentCastEffect);
    }

   
    
  
    public void LevelUpSpell(int slotIndex, int index)
    {
        if (_acquiredSpells.Count > slotIndex)
        {
            SpellInstance spell = _acquiredSpells[slotIndex];

            // Don't level up the weapon if it is already at max level.
            if (!spell.Instance.LevelUp())
            {
                Debug.LogWarning(string.Format(
                    "Failed to level up {0}.",
                    spell.Instance.name
                ));
                return;
            }
        }
    }*/
    
   //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




  
}


using GameDevTV.Inventories;
using InventoryExample.Control;
using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Cauldron : MonoBehaviour
{
    public delegate void ItemAdded(GameDevTV.Inventories.ItemSO item);
    public event ItemAdded OnItemAdded;
   
    public event Action OnPickupSpawned;

    public bool HasPickupSpawned { get => _hasPickupSpawned; set => _hasPickupSpawned = value; }

    
    [SerializeField] List<GameDevTV.Inventories.ItemSO> _currentItems = new List<GameDevTV.Inventories.ItemSO>();
    [SerializeField] List<RecipeSO> _recipes = new List<RecipeSO>();
    [SerializeField] List<Pickup> _items = new List<Pickup>();
    [SerializeField] List<RecipeSO> _pendingRecipes = new List<RecipeSO>();
    [SerializeField] List<RecipeSO> _matchedRecipes = new List<RecipeSO>();
    [SerializeField] MMFeedbacks _feedbacks;
    [SerializeField] [CanBeNull] Material _liquidRenderer;
    [SerializeField] Renderer _bubblesRenderer;
    [SerializeField] Color _defaultLiquidColor;
    
    
    const int MAX_INGREDIENTS = 5;
    
    bool _recipeMatched;
    bool _hasPickupSpawned;

    void Awake()
    {
        
    }

    void Start()
    {
        TimeController.Instance.OnSunrise += () =>
        {
            CheckCraftTimeValidity();
        };
        TimeController.Instance.OnSunset += () =>
        {
            CheckCraftTimeValidity();
        };
        TimeController.Instance.OnTimeStateChanged += (newTimeState) =>
        {
            HandleTimeStateChange(newTimeState);
        };
        SetDefautlLiquidColor();
        SetDefaultBubblesColor();
    }

    void OnDestroy()
    {
        TimeController.Instance.OnSunrise -= CheckCraftTimeValidity;
        TimeController.Instance.OnSunset -= CheckCraftTimeValidity;
        TimeController.Instance.OnTimeStateChanged -= HandleTimeStateChange;
    }

    // ADD ITEMS TO CAULDRON WITH THIS METHOD
    public void AddItem(Pickup item)
    {
        if (_currentItems.Count >= MAX_INGREDIENTS)
        {
            Debug.Log("Cauldron is full!");
            return;
        }
        
        GameDevTV.Inventories.ItemSO itemSO = item.GetItem();
        _currentItems.Add(item.GetItem());
        if (!_recipeMatched)
        {
            CheckRecipes();
        }
        OnItemAdded?.Invoke(itemSO);
    }
    
    public int GetNumberOfItems()
    {
        return _currentItems.Count;
    }

    public bool ContainsItem(GameDevTV.Inventories.ItemSO item)
    {
        return _currentItems.Contains(item);
    }
    
    // You can remove your items from the Cauldron using this method
    public void RemoveItem(GameDevTV.Inventories.ItemSO item)
    {
        _currentItems.Remove(item);
    }

    public int GetCurrentItemsNumber()
    {
        return _currentItems.Count;
    }
    public int GetMaxItemsNumber()
    {
        return MAX_INGREDIENTS;
    }
    
    public void ReceiveOutput(Pickup item)
    {
        _items.Add(item);
    }
    
    RecipeSO GetLastMatchedRecipe()
    {
        // Check if there are any matched recipes
        if (_matchedRecipes.Count == 0)
        {
            // If not, return null
            return null;
        }
        else
        {
            // If there are, return the last one
            return _matchedRecipes[_matchedRecipes.Count - 1];
        }
    }
    
    public void SpawnLastMatchedRecipePickup(Transform objectHolder)
    {
        if (HasPickupSpawned) return; // Ignore if already spawned a pickup
        RecipeSO lastMatchedRecipe = GetLastMatchedRecipe(); 

        // If there's a matched recipe, spawn the Pickup.
        if (lastMatchedRecipe != null)
        {
            // The number of pickups to spawn is the number of items in the recipe.
            Pickup pickup = lastMatchedRecipe.SpawnPickup(objectHolder.position, 1);
        
            // Make the spawned pickup a child of objectHolder.
            pickup.transform.parent = objectHolder;
            
            Player.Instance.HoldObject(pickup);
            
            _hasPickupSpawned = true;
            ClearCauldron();
            // Invoke the event
            ExperienceManager.Instance.AddExperience(lastMatchedRecipe.GetExperienceReward());
            OnPickupSpawned?.Invoke();
        }
    }
    
    void CheckRecipes()
    {
        foreach (var recipe in _recipes)
        {
            if (recipe.IsMatch(_currentItems))
            {
                if (recipe.CanCraftAtThisTimeOfDay())
                {
                    _matchedRecipes.Add(recipe);
                    UpdateCauldronAppearance(recipe);
                    _hasPickupSpawned = false;
                }
                else
                {
                    _pendingRecipes.Add(recipe);  // Add to pending recipes if it's not the right time of day
                }
            }
        } 
    }
    
    void HandleTimeStateChange(TimeController.TimeState newTimeState)
    {
        switch(newTimeState)
        {
            case TimeController.TimeState.EarlyMorning:
                CheckCraftTimeValidity();
                break;
            case TimeController.TimeState.Morning:
                // Do something
                break;
            case TimeController.TimeState.EarlyAfternoon:
                CheckCraftTimeValidity();
                break;
            case TimeController.TimeState.Afternoon:
                CheckCraftTimeValidity();
                break;
            case TimeController.TimeState.Evening:
                CheckCraftTimeValidity();
                break;
            case TimeController.TimeState.LateEvening:
                CheckCraftTimeValidity();
                break;
            case TimeController.TimeState.Midnight:
                CheckCraftTimeValidity();
                break;
            case TimeController.TimeState.LateNight:
                CheckCraftTimeValidity();
                break;
            
            // and so on for each state
        }
    }
    void CheckCraftTimeValidity()
    {
        foreach (var recipe in _pendingRecipes.ToList()) // Create a copy to iterate over
        {
            if (recipe.IsMatch(_currentItems) && recipe.CanCraftAtThisTimeOfDay())
            {
                _matchedRecipes.Add(recipe);
                _pendingRecipes.Remove(recipe);
                UpdateCauldronAppearance(recipe);
                _hasPickupSpawned = false;
            }
        }
    }
    void UpdateCauldronAppearance(RecipeSO recipe)
    {
        _liquidRenderer.SetColor("_BaseColor", recipe.GetPotionColor());
        _liquidRenderer.SetColor("_EmissionColor", recipe.GetPotionColor() * 0.5f);
        _bubblesRenderer.material.SetColor("_BaseColor", recipe.GetPotionColor());
    }

   

    void SetDefautlLiquidColor()
    {
        _liquidRenderer.SetColor("_BaseColor", _defaultLiquidColor);
        _liquidRenderer.SetColor("_EmissionColor", _defaultLiquidColor * 0.5f);
    }
    void SetDefaultBubblesColor()
    {
        _bubblesRenderer.material.SetColor("_BaseColor", _defaultLiquidColor);
    }
    
    void ClearCauldron()
    {
        _currentItems.Clear();
        SetDefautlLiquidColor();
        SetDefaultBubblesColor();
    }
    public void ResetCauldron()
    {
        if (_currentItems.Count == 0) return;
        _feedbacks.PlayFeedbacks();
        SetDefautlLiquidColor();
        SetDefaultBubblesColor();
        ClearCauldron();
        _matchedRecipes.Clear();
        OnPickupSpawned?.Invoke();
    }
}

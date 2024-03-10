using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem
{
    public const int GRID_SIZE = 3;
    
    Ingredient[,] _ingredientArray;  
    
    public CraftingSystem()
    {
        _ingredientArray = new Ingredient[GRID_SIZE,GRID_SIZE];
    }
    
    bool IsEmpty(int x, int y)
    {
        return _ingredientArray[x,y] == null;
    }
    
    Ingredient GetIngredient(int x, int y)
    {
        return _ingredientArray[x,y];
    }
    
    void SetIngredient(int x, int y, Ingredient ingredient)
    {
        _ingredientArray[x,y] = ingredient;
    }
    
    void IncreaseIngredientAmount(int x, int y)
    {
       GetIngredient(x, y).IncreaseIngredientAmount();
    }
    
    void DecreaseIngredientAmount(int x, int y)
    {
        GetIngredient(x, y).DecreaseIngredientAmount();
    }
    
    void RemoveIngredient(int x, int y)
    {
        SetIngredient(x, y, null);
    }
    
    bool TryAddIngredient(Ingredient ingredient, int x, int y)
    {
        if (IsEmpty(x, y))
        {
            SetIngredient(x, y, ingredient);
            return true;
        }
        else
        {
            if (ingredient.GetMaterialType() == GetIngredient(x, y).GetMaterialType())
            {
                IncreaseIngredientAmount(x, y);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
}

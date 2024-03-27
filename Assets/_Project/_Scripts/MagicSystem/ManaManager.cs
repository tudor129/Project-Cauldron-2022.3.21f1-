using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{
    public float MaxMana { get; private set; }
    public float CurrentMana { get; private set; }
    public float RegenRate { get; private set; }
    public float RegenDelay { get; private set; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool ConsumeMana(float amount)
    {
        if (CurrentMana >= amount)
        {
            CurrentMana -= amount;
            return true;
        }
        return false;
    }

}

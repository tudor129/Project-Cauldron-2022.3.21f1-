using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }
    
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one ExperienceManager instance!" + gameObject.name + " is being destroyed!");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddExperience(int amount)
    {
        EventManager<int>.TriggerEvent(EventKey.UPDATE_EXPERIENCE, amount);
    }
}

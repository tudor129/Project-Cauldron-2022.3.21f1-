using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{
    [FormerlySerializedAs("_playerStatSO")]
    [Header("Reference to the PlayerStatsSO")]
    [SerializeField] PlayerData playerData;
    [SerializeField] int _currentLevel;
    [SerializeField] int _maxLevel;
    [SerializeField] public int _currentXP;
    [SerializeField] public int _maxXP;
    [SerializeField] public int _minXP;

    
    
    void Awake()
    {
        playerData.SetPlayerProgression(this);
    }
    void Start()
    {
        EventManager<int>.RegisterEvent(EventKey.UPDATE_EXPERIENCE, HandleExperienceChanged);
    }
    void OnDestroy()
    {
        EventManager<int>.UnregisterEvent(EventKey.UPDATE_EXPERIENCE, HandleExperienceChanged);
    }
    void HandleExperienceChanged(int newExperience)
    {
        if (_currentLevel >= _maxLevel)
        {
            return;
        }
        
        _currentXP += newExperience;  // Update current XP with the new experience

        if (_currentXP >= _maxXP)
        {
            int excessXP = _currentXP - _maxXP;
            LevelUp(excessXP);
        }
    }
    void LevelUp(int excessXP)
    {
        if (_currentLevel < _maxLevel)
        {
            
            StartCoroutine(DelayedLevelUp(excessXP));
        }
    }
    
    IEnumerator DelayedLevelUp(int excessXP)
    {
        yield return new WaitForSeconds(0.5f);
        if (_currentLevel < _maxLevel)
        {
            int newLevel = GetCurrentLevel() + 1;
            _currentLevel = newLevel;
       
            _currentXP = 0;
            _maxXP += 100;
            EventManager<EventArgs>.TriggerEvent(EventKey.UPDATE_LEVEL, EventArgs.Empty);
            EventManager<int>.TriggerEvent(EventKey.UPDATE_MAX_EXPERIENCE, _maxXP);
            EventManager<int>.TriggerEvent(EventKey.UPDATE_EXPERIENCE_AFTER_LEVEL_UP, excessXP);
            EventManager<int>.TriggerEvent(EventKey.UPDATE_EXPERIENCE, _currentXP);
        }
        
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }
    public int GetCurrentXp()
    {
        return _currentXP;
    }
     
    
   
   

    
}

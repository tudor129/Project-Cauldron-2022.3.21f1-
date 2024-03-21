using System;
using UnityEngine;
public class SpellBehavior : MonoBehaviour
{
    public Spell spell;
    public PlayerData playerData;
    
    protected Player _player;
    protected ParticleSystem _part;
    
    
    
    
    
    public Spell.Stats _currentStats;
    
    protected virtual void Awake()
    {
        _player = GameManager.Instance.player;
        
      
        //Debug.Log("Current stats for ImpactLifetime in awake: " + _currentStats.SpellPrefab.name);
        // _currentStats = spell.GetStats();
        // Debug.Log("Current stats for ImpactLifetime in awake: " + _currentStats.SpellPrefab.name);
        
    }

    protected virtual void Start()
    {
        // if (!_isInitialized)
        // {
        //     _currentStats = spell.GetStats();
        // }
        
       
    }
    
   
}

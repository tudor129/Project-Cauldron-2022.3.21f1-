using System;
using UnityEngine;
public class BaseSpellBehavior : MonoBehaviour
{
    public Spell spell;
    public PlayerData playerData;
    
    protected Player _player;
    protected ParticleSystem _part;
    protected BoxCollider _collider;
    
    public Spell.Stats _currentStats;
    bool _isInitialized;

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
    
    public void Initialize(Spell spell)
    {
        if (_isInitialized) return;  // Prevent re-initialization

        
        this.spell = spell;
        _currentStats = spell.GetStats();
        _isInitialized = true;

        

        // Perform any operations previously in Awake or Start that depend on initialization here
        PostInitialization();
    }

    void PostInitialization()
    {
        
    }
}
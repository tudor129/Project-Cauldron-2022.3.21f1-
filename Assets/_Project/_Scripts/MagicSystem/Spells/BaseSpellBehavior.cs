using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
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
    }

    protected virtual void Start()
    {
   
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
    
    protected IEnumerator ReturnToPoolAfterDelay(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);
        ObjectPoolManager.Instance.ReturnObjectToPool(objectToReturn);
    }
}
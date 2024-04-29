using MoreMountains.Feedbacks;
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
    protected MMF_Player _feedback;
    
    public Spell.Stats _currentStats;
    protected bool _isInitialized;

    protected virtual void Awake()
    {
        _player = GameManager.Instance.player;
    }

    protected virtual void Start()
    {
   
    }
    
    public void Initialize(Spell spell, MMF_Player feedback)
    {
        if (_isInitialized) return;  // Prevent re-initialization

        
        this.spell = spell;
        _currentStats = spell.GetStats();
        _feedback = feedback;
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
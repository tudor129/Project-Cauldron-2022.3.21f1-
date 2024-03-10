using System;
using UnityEngine;
public class SpellBehavior : MonoBehaviour
{
    public Spell spell;
    public PlayerData playerData;
    
    protected Player _player;
    protected ParticleSystem _part;
    
    
    
    protected Spell.Stats _currentStats;
    
    protected virtual void Awake()
    {
        _player = GameManager.Instance.player;
    }

    protected virtual void Start()
    {
        _currentStats = spell.GetStats();
    }
}

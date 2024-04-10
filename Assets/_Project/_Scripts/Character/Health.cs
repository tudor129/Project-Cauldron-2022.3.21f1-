using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour, IAttackable
{
    [SerializeField] protected int _maxHealth = 100;

    protected CharacterAnimatorManager _animatorManager;
    protected Collider _collider;
    
    protected int _currentHealth;
    protected bool _isAlive;

    protected virtual void Awake()
    {
        _animatorManager = GetComponent<CharacterAnimatorManager>();
        _collider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
        _collider.enabled = true;
    }
    
    protected virtual void Die()
    {
        if (_animatorManager != null)
        {
            _animatorManager.StopAttackAnimation();
            _animatorManager.StopWalkingAnimation();
            _animatorManager.PlayDeathAnimation();
        }
        
        _collider.enabled = false;
    }
    
    public bool IsDead()
    {
        return _currentHealth <= 0;
    }
    
    // This is a workaround for the fact that the Animation Event system doesn't support bool parameters
    public virtual void TakeDamage(int amount, bool isCritical, Spell.Stats spellInfo, bool isDirectDamage = true)
    {
        TakeDamage(amount); // call the non-critical version by default
    }
    public bool IsActive()
    {
        return !IsDead() && gameObject.activeSelf;
    }
    public virtual void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        if (_animatorManager != null)
        {
            _animatorManager.PlayTakeDamageAnimation();
        }

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public void Respawn()
    {
        ResetHealth();
        _animatorManager.Revive();
        _collider.enabled = true;
    }
}

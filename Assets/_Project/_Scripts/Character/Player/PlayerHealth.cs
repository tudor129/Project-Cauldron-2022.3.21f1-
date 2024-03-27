using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    
    [SerializeField] HealthBarUI _healthBarUI;
    [SerializeField] HealthBallUI _healthBallUI;
    
    PlayerStats _playerStats;
    PlayerData _playerStat;


    protected override void Awake()
    {
        base.Awake();
        EventManager<EventArgs>.RegisterEvent(EventKey.UPDATE_LEVEL, OnPlayerLevelUp);
    }
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.SetPlayerStats(this);
        EventManager<int>.TriggerEvent(EventKey.UPDATE_HEALTH, _currentHealth);
        //_healthBarUI.SetMaxHealth(_playerStat.GetMaxHealth());
        //_healthBarUI.SetCurrentHealth(_currentHealth);
    }
    void OnDestroy()
    {
        EventManager<EventArgs>.UnregisterEvent(EventKey.UPDATE_LEVEL, OnPlayerLevelUp);
    }
    protected override void Die()
    {
        base.Die();
        InputManager.Instance.gameObject.SetActive(false);
        // Delete this after actual implementation
        gameObject.SetActive(false);
    }

    public override void TakeDamage(int amount)
    {
        if (Player.Instance._isInvulnerable)
        {
            return;
        }
        base.TakeDamage(amount);
        EventManager<int>.TriggerEvent(EventKey.UPDATE_HEALTH, _currentHealth);
    }
    
    void OnPlayerLevelUp(EventArgs args)
    {
        int maxHealth = _playerStat.GetMaxHealth();
        Debug.Log("Player's max health is now: " + maxHealth);
    }
    
    public void SetPlayerStatForHealth(PlayerData playerStat)
    {
        _playerStat = playerStat;
    }
}

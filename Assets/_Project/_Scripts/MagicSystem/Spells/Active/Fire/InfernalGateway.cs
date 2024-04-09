using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class InfernalGateway : Spell
{
    protected override void Awake()
    {
        base.Awake();
    }
        
    protected override void Start()
    {
        base.Start();

        // spellBehavior = Instantiate(_currentStats.SpellPrefab, _player.transform.position + new Vector3(0, 10, 0), Quaternion.identity);
    }
    
    protected override void Update()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0f) //Once the cooldown becomes 0, attack
        {
            Attack(_currentStats.NumberOfAttacks);
        }
    }
    
    protected override bool Attack(int attackCount = 1)
    {
        // If no projectile prefab is assigned, leave a warning message.
        if (!_currentStats.SpellPrefab)
        {
            Debug.LogWarning(string.Format("Projectile prefab has not been set for {0}", name));
            _currentCooldown = _currentStats.Cooldown;
            return false;
        }

        // Can we attack?
        if (!CanAttack()) return false;
       

        // And spawn a copy of the projectile.
        SpellBehavior prefab = Instantiate(_currentStats.SpellPrefab, _player.transform.position + _currentStats.ImpactOffset, Quaternion.identity);
        
        prefab.Initialize(this);
      
        /*Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;

                Transform found = FindDeepChild(child, name);
                if (found != null)
                    return found;
            }
            return null;
        }*/
        
        // Transform lava = FindDeepChild(prefab.transform, "Lava");
        // Transform rain = FindDeepChild(prefab.transform, "Rain");
        
        Transform lava = prefab.transform.FindDeepChild("Lava");
        Transform rain = prefab.transform.FindDeepChild("Rain");
        
        
        // This actually sets the radius of the lava pool. Set it to 1 for the minimum size. Don't set higher than 4.
        rain.transform.localScale = new Vector3(_currentStats.SpellRadius, _currentStats.SpellRadius, _currentStats.SpellRadius);
        
        lava.SetParent(null);
        rain.GetComponent<RainBehavior>().Initialize(this);
        
        Destroy(prefab.gameObject, _currentStats.Lifetime);
        Destroy(lava.gameObject, _currentStats.DecalLifetime);

        // Reset the cooldown only if this attack was triggered by cooldown.
        if (_currentCooldown <= 0)
            _currentCooldown += _currentStats.Cooldown;

        return true;
    }
    
    void UpdateParticleSystemDuration(ParticleSystem particleSystem, float newDuration)
    {
        // Stop the particle system with clearing all particles
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Update the duration
        var mainModule = particleSystem.main;
        mainModule.duration = newDuration;

        // Restart the particle system
        particleSystem.Play();
    }
}

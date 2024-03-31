using System.Collections.Generic;
using UnityEngine;
public class PassiveBaseSpellBehavior : BaseSpellBehavior
{
    public LightningStrikeData lightningStrikeData;
    
    protected int _hitCount;
    
    List<Collider> _enemyColliders = new List<Collider>();
    
    float _enemyDetectionRadius = 20f;
    float _range = 10f;
    
    Vector3 _spawnPosition;
    Quaternion _spawnRotation;

    protected override void Awake()
    {
        _enemyColliders.Clear();
        base.Awake();
       
    }

    protected override void Start()
    {
        _part = GetComponent<ParticleSystem>();
        //Initialize(lightningStrikeData, _player, PlayerData);
        _spawnPosition = GameManager.Instance._playerCastPoint.transform.position;
        _spawnRotation = transform.rotation;
        _currentStats = spell.GetStats();
        
        
        Vector3? randomEnemyPosition = FindRandomEnemyPosition();
        if (randomEnemyPosition.HasValue)
        {
            transform.position = randomEnemyPosition.Value;
            DealDamage(_currentStats);
        }
        else
        {
            Destroy(gameObject);
        }
        
        Destroy(gameObject, 2);
    }

    protected  void Update()
    {
        
    }
    
    Vector3? FindRandomEnemyPosition()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _enemyDetectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                _enemyColliders.Add(hitCollider);
            }
        }

        if (_enemyColliders.Count > 0)
        {
            // Select a random enemy
            int randomIndex = UnityEngine.Random.Range(0, _enemyColliders.Count);
            return _enemyColliders[randomIndex].transform.position;
        }

        // // Return null if no enemies are found
         return null;
        
    }
    
    void DealDamage(Spell.Stats spellData)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 4f, Vector3.down, out hit, _range))
        {
            if (hit.collider.gameObject.TryGetComponent(out IAttackable attackable))
            {
                // Apply damage
                (int damage, bool isCritical) = CalculateAttackDamage(_hitCount, 0.1f, 3, spellData);
                attackable.TakeDamage(damage, isCritical, spellData);
            }
        }
    }


    protected virtual (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, Spell.Stats spellData)
    {
        // Random range between base damage and base damage + 15% of base damage
        int damageOutput = Random.Range(spellData.Damage, spellData.Damage + (int)(spellData.Damage * 0.15f));
        
        
        bool isCriticalHit = false;
        
        // Calculate critical hit
        float roll = UnityEngine.Random.value; // Get a random value between 0.0 and 1.0
        if (roll <= critChance) // If the random value is less than or equal to the crit chance, a critical hit occurs
        {
            damageOutput = (int)(spellData.Damage * critMultiplier); // Multiply the damage by the critical hit multiplier
            isCriticalHit = true;
        }
        return (damageOutput, isCriticalHit);
    }
}

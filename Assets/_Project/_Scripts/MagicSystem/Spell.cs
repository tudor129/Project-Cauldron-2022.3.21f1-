using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;


public abstract class Spell : Item, ISpell
{
    [System.Serializable]
    public struct Stats
    {
        public string Name;
        public string Description;
        
        [Header("Visuals")]
        public ProjectileBehavior SpellPrefab;
        public SpellImpactBehavior SpellImpactPrefab;
        public GameObject CharacterEffect;
        public VisualEffect VisualEffect;
        public ParticleSystem DetachEffect;
        public bool HasFlash;
        public bool HasCastEffect;
        
        public string SpellAnimation;
        
        [Header("Sound")]
        public AudioClip CastSound;
        public AudioClip TravelingSound;
        public AudioClip LoopingSound;
        public AudioClip ImpactSound;
        [Header("Sound arrays for variations. If not used, leave empty.")]
        public AudioClip[] CastSounds;
        public AudioClip[] TravelingSounds;
        public AudioClip[] LoopingSounds;
        public AudioClip[] ImpactSounds;
        
        
        [Header("Spell Type")]
        public TypeOfSpell SpellType;
        public enum TypeOfSpell
        {
            AutoCast,
            ManualCast,
        }
        [Header("Values")]
        [Tooltip("The offset from the impact point where the SpellImpactPrefab will be instantiated. Use this to avoid z-fighting.")]
        public Vector3 ImpactOffset;
        public int Damage;
        public float Speed;
        public float ProjectileLifetime;
        public float ImpactLifetime;
        public float SpellRadius;
        public float Cooldown;
        public int DamageOverTimeDamage;
        public float DamageOverTimeDuration;
        public float DamageOverTimeInterval;
        public float DamageOverTimeInitialDelay;
        public float StaggerDuration;
        [Tooltip("If true, the spell will spawn projectiles sequentially, with a delay between each spawn. If false, all projectiles will spawn at the same time.")]
        public bool SpawnsProjectilesSequentially;
        [Tooltip("If true, the spell will spawn projectiles backwards, in the opposite direction of the player's forward.")]
        public bool ShootBackwards;
        public int NumberOfAttacks;
        public float ProjectileInteval;
        [Tooltip("The angle step between each projectile, in degrees. Used to spread out the projectiles. If NumberOfAttacks is 1, this value will be ignored.")]
        public float AngleStep;
        [Tooltip("The radius of the spread, in meters. Used to spread projectile location from the player. If NumberOfAttacks is 1, this value will be ignored.")]
        public float SpreadRadius;
        
        [Header("Affinity")]
        public bool IsFire;
        public bool IsIce;
        public bool IsLightning;
        public bool IsPoison;
        public bool IsArcane;
        
        public bool DoesDoT;
        public bool IsPassThrough;
        
        public static Stats operator +(Stats s1, Stats s2)
        {
            Stats result = new Stats();
            result.Name = s2.Name ?? s1.Name;
            result.Description = s2.Description ?? s1.Description;
            result.SpellPrefab = s2.SpellPrefab ?? s1.SpellPrefab;
            result.SpellImpactPrefab = s2.SpellImpactPrefab ?? s1.SpellImpactPrefab;
            result.CharacterEffect = s2.CharacterEffect ?? s1.CharacterEffect;
            result.VisualEffect = s2.VisualEffect ?? s1.VisualEffect;
            result.HasFlash = s2.HasFlash || s1.HasFlash;
            result.HasCastEffect = s2.HasCastEffect || s1.HasCastEffect;
            result.SpellAnimation = s2.SpellAnimation ?? s1.SpellAnimation;
            result.CastSound = s2.CastSound ?? s1.CastSound;
            result.TravelingSound = s2.TravelingSound ?? s1.TravelingSound;
            result.LoopingSound = s2.LoopingSound ?? s1.LoopingSound;
            result.ImpactSound = s2.ImpactSound ?? s1.ImpactSound;
            result.CastSounds = s2.CastSounds ?? s1.CastSounds;
            result.TravelingSounds = s2.TravelingSounds ?? s1.TravelingSounds;
            result.LoopingSounds = s2.LoopingSounds ?? s1.LoopingSounds;
            result.ImpactSounds = s2.ImpactSounds ?? s1.ImpactSounds;
            result.SpellType = s2.SpellType;
            result.Damage = s1.Damage + s2.Damage;
            result.Speed = s1.Speed + s2.Speed;
            result.ProjectileLifetime = s1.ProjectileLifetime + s2.ProjectileLifetime;
            result.ImpactLifetime = s1.ImpactLifetime + s2.ImpactLifetime;
            result.SpellRadius = s1.SpellRadius + s2.SpellRadius;
            result.Cooldown = s1.Cooldown + s2.Cooldown;
            result.DamageOverTimeDamage = s1.DamageOverTimeDamage + s2.DamageOverTimeDamage;
            result.DamageOverTimeDuration = s1.DamageOverTimeDuration + s2.DamageOverTimeDuration;
            result.DamageOverTimeInterval = s1.DamageOverTimeInterval + s2.DamageOverTimeInterval;
            result.DamageOverTimeInitialDelay = s1.DamageOverTimeInitialDelay + s2.DamageOverTimeInitialDelay;
            result.StaggerDuration = s1.StaggerDuration + s2.StaggerDuration;
            result.SpawnsProjectilesSequentially = s2.SpawnsProjectilesSequentially || s1.SpawnsProjectilesSequentially;
            result.ShootBackwards = s2.ShootBackwards || s1.ShootBackwards;
            result.NumberOfAttacks = s1.NumberOfAttacks + s2.NumberOfAttacks;
            result.ProjectileInteval = s1.ProjectileInteval + s2.ProjectileInteval;
            result.AngleStep = s1.AngleStep + s2.AngleStep;
            result.SpreadRadius = s1.SpreadRadius + s2.SpreadRadius;
            result.IsFire = s2.IsFire || s1.IsFire;
            result.IsIce = s2.IsIce || s1.IsIce;
            result.IsLightning = s2.IsLightning || s1.IsLightning;
            result.IsPoison = s2.IsPoison || s1.IsPoison;
            result.IsArcane = s2.IsArcane || s1.IsArcane;
            result.DoesDoT = s2.DoesDoT || s1.DoesDoT;
            result.IsPassThrough = s2.IsPassThrough || s1.IsPassThrough;
            
               
            return result;
        }
    }
    
  
    protected Stats _currentStats;
    
    
    
    
    [FormerlySerializedAs("SpellSO")] public SpellData spellData;
    
    protected PlayerData PlayerData;
  
    protected SphereCollider _sphereCollider;
    protected Rigidbody _rigidbody;
    protected Vector3 _spawnPosition;
    protected Quaternion _spawnRotation;
    
    protected ParticleSystem _part;
    protected List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();
    protected ParticleSystem _ps;

    protected int _hitCount;
    readonly int _value;
    
    

    protected virtual void Awake()
    {
        if (spellData)
        {
            _currentStats = spellData.BaseStats;
        }
    }
    
    protected virtual void Start()
    {
        if (spellData)
        {
            InitializeData(spellData);
        }
    }

    protected virtual void Update()
    {
                
    }

    protected virtual (int, bool) CalculateAttackDamage(int hitCount, float critChance, float critMultiplier, SpellData spellInfo)
    {
        int damage = spellInfo.BaseStats.Damage;
        bool isCrit = UnityEngine.Random.value < critChance;
        if (isCrit)
        {
            damage = Mathf.RoundToInt(damage * critMultiplier);
        }
        return (damage, isCrit);
    }

    public void InitializeData(SpellData data)
    {
        MaxLevel = data.MaxLevel;
        
        this.spellData = data;
        _currentStats = data.BaseStats;
        _player = GameManager.Instance.player;
        
    }
    
    public void Initialize(SpellData spellData, Player playerTransform, PlayerData playerStats)
    {
        this.spellData = spellData;
        _currentStats = spellData.BaseStats;
        
        _player = GameManager.Instance.player;
        PlayerData = GameManager.Instance._playerStat;
        
    }
    
    public virtual bool CanLevelUp()
    {
        return CurrentLevel <= MaxLevel;
    }
    
    public virtual bool LevelUp()
    {
        
        if (!CanLevelUp())
        {
            Debug.LogWarning(string.Format("Cannot level up {0} to Level {1}, max level of {2} already reached.", name, CurrentLevel, spellData.MaxLevel));
            return false;
        }
        CurrentLevel++;
        _currentStats += spellData.GetLevelData(CurrentLevel);
        Debug.Log("Leveling up" + name + " to level " + CurrentLevel);
        Debug.Log("New stats: " + _currentStats.SpellPrefab.name);
        return true;
    }

    
    public virtual Stats GetStats()
    {
        return _currentStats;
    }

    public GameObject GetSpellGameObject()
    {
        return gameObject;
    }


}
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class AbilityData : ItemData
{
    public GameObject AbilityEffectsPrefab;
    public GameObject AbilityStartEffect;
    public VisualEffect VisualEffectGraph;
    public Material AbilityMaterial;
    public GameObject FirePrefab;
    
    [Header("Ability Type")]
    public TypeOfAbility AbilityType;
    
    [Header("Ability Affinity")]
    public bool IsFireDash;
    public bool IsIceDash;
    public bool IsLightningDash;
    public bool IsPoisonDash;
    public bool IsArcaneDash;
    
    [Header("Ability Stats")]
    public int DamageOutput;
    public int BaseDamage = 0;
    public float Speed = 20;
    public float AbilityRadius = 0;
    public float PushForce = 0;
    public float Cooldown = 1;
    public int NumberOfCharges = 1;
    public float Range = 5;
    public float Duration = 1;
    public float IFramesTime = 0.5f;
    
    [Header("Trail Stats")]
    [Tooltip("Keep this value at 0.25f for best effect")]
    public float TrailVisibleTime = 0.5f;
    [Tooltip("Keep this value at 0.025f for best effect")]
    public float TrailRefreshRate = 0.05f;
    public float ElementTrailRefreshRate = 0.05f;
    
    [Header("Trail Shader Stats")]
    public string ShaderVarRef = "_Alpha";
    public float ShaderVarRate = 0.1f;
    public float ShaderVarRefreshRate = 0.05f;
    
    
    public enum TypeOfAbility
    {
        Dash
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lightning Cloud Spell", menuName = "Spells/Lightning Cloud Spell")]
public class LightningCloudData : SpellData
{
    [Tooltip("The radius of the circle around the player the spell will wander around in")]
    public float WanderRadius = 5f;
    public float WanderTimer = 5f;
    public float MoveTime = 1.0f;
    public float EnemyCheckTimer = 1f;
    public float EnemyDetectionRadius = 10f;
    
}

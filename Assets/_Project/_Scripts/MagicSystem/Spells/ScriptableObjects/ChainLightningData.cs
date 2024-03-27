using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;



[CreateAssetMenu(fileName = "New Chain Lightning Spell", menuName = "Spells/Chain Lightning Spell")]
public class ChainLightningData : SpellData
{
    public GameObject BeenStruckEffect;
    
    [Tooltip("How many targets the lightning can jump to")]
    public int ChainCount = 5;
    [Tooltip("How far the lightning can travel from one target to the next")]
    public float ChainRange = 10f;
    [Tooltip("How quickly the lightning travels from one target to the next")]
    public float ChainSpeed = 0.1f;
}

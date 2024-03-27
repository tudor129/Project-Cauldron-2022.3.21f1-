using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wilfire Leap Spell", menuName = "Spells/Wildfire Leap Spell")]
public class WildfireLeapData : SpellData
{
   
    
    [Tooltip("How many targets the projectile can jump to")]
    public int LeapCount = 5;
    [Tooltip("How far the projectile can travel from one target to the next")]
    public float LeapRange = 10f;
    [Tooltip("How quickly the lightning travels from one target to the next")]
    public float LeapSpeed = 1f;
}

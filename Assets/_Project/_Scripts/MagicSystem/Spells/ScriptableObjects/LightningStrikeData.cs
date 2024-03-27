using UnityEngine;


[CreateAssetMenu(fileName = "New Lightning Strike Spell", menuName = "Spells/Lightning Strike Spell")]
public class LightningStrikeData : SpellData
{
    [Tooltip("The distance the player needs to travel before the spell is cast")]
    public float PlayerTravelDistance = 10f;
}

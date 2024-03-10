/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DealDamage : MonoBehaviour
{
    [FormerlySerializedAs("SpellSO")] public SpellData spellData;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }
        IAttackable attackable = other.GetComponent<IAttackable>();
        if (attackable != null)
        {
            attackable.TakeDamage(spellData.BaseDamage, false, spellData);
        }
    }
}
*/

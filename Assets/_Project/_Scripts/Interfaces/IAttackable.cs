using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void TakeDamage(int amount, bool isCritical, Spell.Stats spellData, bool isDirectDamage = true);
    bool IsActive();
}

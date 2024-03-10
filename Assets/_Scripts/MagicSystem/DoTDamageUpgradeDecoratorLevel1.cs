using System;
using UnityEngine;
public class DoTDamageUpgradeDecoratorLevel1 : StatUpgradeDecorator
{
    public float Modifier;
    public DoTDamageUpgradeDecoratorLevel1(SpellInstance spellToDecorate, SpellStatUpgradeData spellStatUpgrade, float modifier) : base(spellToDecorate)
    {
        //modifier = Modifier;
        
        CopyStateFrom(spellToDecorate);
        ApplyUpgrade(spellStatUpgrade);
        
    }
    
    public override void ApplyUpgrade(SpellStatUpgradeData statUpgrade)
    {
        base.ApplyUpgrade(statUpgrade);
        
        DamageOverTimeDamage = Mathf.RoundToInt(DamageOverTimeDamage * statUpgrade.ModifierLevel2);
        
        Debug.Log($"Spell {Spell.name} upgraded with {statUpgrade.name}.");
        Debug.Log($"Spell {Spell.name} damage over time is now {DamageOverTimeDamage}.");
    }
    
    
}
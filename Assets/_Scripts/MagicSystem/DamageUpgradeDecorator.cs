using UnityEngine;
public class DamageUpgradeDecorator : StatUpgradeDecorator
{
    public float DamageModifier;
    
    public DamageUpgradeDecorator(SpellInstance spellToDecorate, SpellStatUpgradeData spellStatUpgrade, float damageModifier) : base(spellToDecorate)
    {
        damageModifier = DamageModifier;
        
        CopyStateFrom(spellToDecorate);
        ApplyUpgrade(spellStatUpgrade);
        
        
    }
    
   
    
    public override void ApplyUpgrade(SpellStatUpgradeData statUpgrade)
    {
        base.ApplyUpgrade(statUpgrade);
        
        
        Damage = Mathf.RoundToInt(Damage * statUpgrade.Modifier);
        
        Debug.Log($"Spell {Spell.name} upgraded with {statUpgrade.name}.");
        Debug.Log($"Spell {Spell.name} damage is now {Damage}.");
    }
}

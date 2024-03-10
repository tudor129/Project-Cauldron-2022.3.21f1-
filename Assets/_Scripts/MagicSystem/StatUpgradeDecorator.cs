public abstract class StatUpgradeDecorator : SpellInstance
{
    protected SpellInstance _spellToDecorate;
    
    protected StatUpgradeDecorator(SpellInstance spellToDecorate) : base(spellToDecorate.Spell)
    {
        _spellToDecorate = spellToDecorate;
        CopyStateFrom(spellToDecorate);
    }
    
    protected void CopyStateFrom(SpellInstance source)
    {
        Spell = source.Spell;
        Damage = source.Damage;
        Speed = source.Speed;
        Lifetime = source.Lifetime;
        SpellRadius = source.SpellRadius;
        Cooldown = source.Cooldown;
        DamageOverTimeDamage = source.DamageOverTimeDamage;
        DamageOverTimeDuration = source.DamageOverTimeDuration;
        DamageOverTimeInterval = source.DamageOverTimeInterval;
        DamageOverTimeInitialDelay = source.DamageOverTimeInitialDelay;
        StaggerDuration = source.StaggerDuration;
    }
  
    
    public override void ApplyUpgrade(SpellStatUpgradeData statUpgrade)
    {
        base.ApplyUpgrade(statUpgrade);
    }
    
    
    
}
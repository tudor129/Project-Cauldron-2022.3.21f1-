using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernalGateway : Spell
{
    
    
    protected override void Awake()
    {
        base.Awake();
    }
        
    protected override void Start()
    {
        base.Start();

        SpellBehavior spellBehavior = Instantiate(_currentStats.SpellPrefab, _player.transform.position + new Vector3(0, 10, 0), Quaternion.identity);
    }
    
    protected override void Update()
    {
        
    }
}

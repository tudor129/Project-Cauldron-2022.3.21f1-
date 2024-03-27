using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell
{
    void Initialize(SpellData spellData, Player playerTransform, PlayerData playerStats);
    
    GameObject GetSpellGameObject();

}
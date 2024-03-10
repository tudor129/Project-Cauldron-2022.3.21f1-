using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This class is obsolete, we are using the Attack method in the BaseActiveSpell class and derived classes.")]
public class SpellFactory : ScriptableObject
{


    /*public static ISpell CreateSpell(SpellInstance spellInstance)
   {
       //Debug.Log("SpellFactory1.CreateSpell");
       if (spellInstance == null)
       {
           Debug.Log("Spell slot is empty");
           return null;
       }
       GameObject spellPrefab = spellInstance.Spell.SpellProjectileFX;
       GameObject spellObject = Instantiate(spellPrefab);
       ISpell spell = spellObject.GetComponent<ISpell>();
       if (spell == null)
       {
           throw new InvalidOperationException("The spell prefab must have a component that implements ISpell");
       }
       Debug.Log("Spell1 created" + spellObject.name);
       //spell.Initialize(spellInstance);
       return spell;
   }
    
    
    public static ISpell CreateSpell(SpellData spellData, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        Debug.Log("SpellFactory2.CreateSpell");
        if (spellData == null)
        {
            Debug.Log("Spell slot is empty");
            return null;
        }
        GameObject spellPrefab = spellData.SpellProjectileFX; 
        GameObject spellObject = Instantiate(spellPrefab, spawnPosition, spawnRotation);
        ISpell spell = spellObject.GetComponent<ISpell>();
        if (spell == null)
        {
            throw new InvalidOperationException("The spell prefab must have a component that implements ISpell");
        }
        Debug.Log("Spell2 created" + spellObject.name);
        return spell;
    }*/
    
    // Make a method to clear static data
    // This is useful for when we reload the scene   
    
    
}

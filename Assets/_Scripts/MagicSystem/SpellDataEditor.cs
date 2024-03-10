using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(SpellData))]
public class WeaponDataEditor: Editor
{
    SpellData _spellData;
    string[] _spellSubtypes;
    int _selectedSpellSubtype;

    void OnEnable()
    {
        // Cache the weapon data value.
        _spellData = (SpellData)target;

        // Retrieve all the weapon subtypes and cache it.
        System.Type baseType = typeof(Spell);
        List<System.Type> subTypes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToList();

        // Add a None option in front.
        List<string> subTypesString = subTypes.Select(t => t.Name).ToList();
        subTypesString.Insert(0, "None");
        _spellSubtypes = subTypesString.ToArray();

        // Ensure that we are using the correct weapon subtype.
        _selectedSpellSubtype = Math.Max(0, Array.IndexOf(_spellSubtypes, _spellData.Behaviour));
    }

    public override void OnInspectorGUI()
    {
        // Draw a dropdown in the Inspector
        _selectedSpellSubtype = EditorGUILayout.Popup("Behaviour", Math.Max(0, _selectedSpellSubtype), _spellSubtypes);

        if (_selectedSpellSubtype > 0)
        {
            // Updates the behaviour field.
            _spellData.Behaviour = _spellSubtypes[_selectedSpellSubtype].ToString();
            EditorUtility.SetDirty(_spellData); // Marks the object to save.
            DrawDefaultInspector(); // Draw the default inspector elements
        }
    }
}

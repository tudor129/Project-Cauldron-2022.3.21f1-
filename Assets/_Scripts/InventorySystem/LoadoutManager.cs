using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
public class LoadoutManager : MonoBehaviour
{
    [SerializeField, Required] HotBar _hotBar;
    [SerializeField] Button[] _loadoutButtons;
    [SerializeField] Button _saveButton;
    
    readonly HotBar.Memento[] _loadoutMementos = new HotBar.Memento[3];

    int _selectedLoadout = 0;

    void Start()
    {
        for (int i = 0; i < _loadoutMementos.Length; i++)
        {
            _loadoutMementos[i] = _hotBar.CreateMemento();
            var index = i;
            _loadoutButtons[i].onClick.AddListener(() => SelectLoadout(index));
        }
        
        _saveButton.onClick.AddListener(SaveLoadout);
    }
    void SelectLoadout(int index)
    {
        SaveLoadout();
        _selectedLoadout = index;
        _hotBar.SetMemento(_loadoutMementos[index]);
    }
    
    void SaveLoadout()
    {
        _loadoutMementos[_selectedLoadout] = _hotBar.CreateMemento();
    }
    
}

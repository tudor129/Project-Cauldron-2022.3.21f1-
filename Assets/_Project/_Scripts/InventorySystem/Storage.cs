using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Storage : MonoBehaviour
{
    [Tooltip("Static storage cannot be swapped out - for Ability Trees, Spellbooks, etc")]
    public bool StaticStorage = false;
    
    [SerializeField] protected List<UISlot> _slots = new List<UISlot>();
    [SerializeField] protected List<ItemData> _items = new List<ItemData>();

    UISlot _swapUISlot;
    
    

    protected virtual void Start()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].UpdateUI(_items[i]);
            _slots[i].SetupStorage(this);
            _slots[i].SetupMouseDrag(this);
        }
    }

    public void SwapItem(UISlot slot)
    {
        
        if (_swapUISlot == null)
        {
            _swapUISlot = slot;
        }
        else if(_swapUISlot == slot)
        {
            _swapUISlot = null;
        }
        else
        {
            Storage storage1 = _swapUISlot.GetStorage();
            int index1 = storage1.GetItemIndex(_swapUISlot);
            ItemData item1 = storage1.GetItem(index1);
            
            Storage storage2 = slot.GetStorage();
            int index2 = storage2.GetItemIndex(slot);
            ItemData item2 = storage2.GetItem(index2);

            if (!storage1.StaticStorage)
            {
                storage1.SetItemSlot(index1, item2);
            }
            
            if (!storage2.StaticStorage)
            {
                storage2.SetItemSlot(index2, item1);
            }
            
            if (!storage1.StaticStorage)
            {
                _swapUISlot?.UpdateUI(item2);
                slot.UpdateUI(item1);
            }
            
            _swapUISlot = null;
        }
    }
    public void ClearSwap()
    {
        _swapUISlot = null;
    }
    
    public int GetItemIndex(UISlot slot)
    {
        return _slots.IndexOf(slot);
    }
    public ItemData GetItem(int index)
    {
        return _items[index];
    }
    void SetItemSlot(int index, ItemData itemData)
    {
        _items[index] = itemData;
    }
    
   
}
using GameDevTV.Inventories;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MonoBehaviour
{
    [SerializeField] GameObject _itemIconPrefab;
    [SerializeField] Transform _itemIconParent;
    [SerializeField] Cauldron _cauldron;

    List<Image> _itemIcons = new List<Image>();
    void OnEnable()
    {
        if (_cauldron != null)
        {
            _cauldron.OnItemAdded += AddItem;
            _cauldron.OnPickupSpawned += ClearItems;
        }
        
    }
    void OnDisable()
    {
        if (_cauldron != null)
        {
            _cauldron.OnItemAdded -= AddItem;
            _cauldron.OnPickupSpawned -= ClearItems;
        }
    }

    

    void AddItem(GameDevTV.Inventories.ItemSO item)
    {
        GameObject iconGO = Instantiate(_itemIconPrefab, _itemIconParent);
        Image itemIcon = iconGO.transform.GetChild(0).GetComponent<Image>();  // Get Image component from the child objects

        if (itemIcon != null && item != null)
        {
            itemIcon.sprite = item.GetIcon();
            _itemIcons.Add(iconGO.GetComponent<Image>());
        }
    }

    void ClearItems()
    {
        foreach (var itemIcon in _itemIcons)
        {
            Destroy(itemIcon.gameObject);
        }
        _itemIcons.Clear();
    }
}

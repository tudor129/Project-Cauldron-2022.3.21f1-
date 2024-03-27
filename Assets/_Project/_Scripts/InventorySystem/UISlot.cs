using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

public class UISlot : MonoBehaviour, IPointerClickHandler
{
    public Image ItemImage;
    Storage _storage;
    UpgradeScreen _upgradeScreen;
    MouseDrag _mouseDrag;
    public Button _button;
    

    void Awake()
    {
        ItemImage = GetComponentInChildren<Image>();
    }

    void Start()
    {
        ItemImage.color = Color.white;
        _button = GetComponentInChildren<Button>();
        if (_button != null)
        {
            _button.onClick.AddListener(OnClick);
        }
    }
    
    public void SetupStorage(Storage storage)
    {
        _storage = storage;
    }
    public Storage GetStorage()
    {
        return _storage;
    }
    
    public void SetupUpgradeScreen(UpgradeScreen upgradeScreen)
    {
        _upgradeScreen = upgradeScreen;
    }
    public UpgradeScreen GetUpgradeScreen()
    {
        return _upgradeScreen;
    }
   
    public void UpdateUI(ItemData itemData)
    {
        if (itemData == null)
        {
            ItemImage.sprite = null;
        }
        else
        {
            ItemImage.sprite = itemData.icon;
        }
    }
    public void SetupMouseDrag(Storage storage)
    {
        _mouseDrag = gameObject.GetOrAdd<MouseDrag>();
        _mouseDrag.SetupStorage(storage, this);
    }
    
    public void OnClick()
    {
        _upgradeScreen.OnSlotClick(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
    
   


}

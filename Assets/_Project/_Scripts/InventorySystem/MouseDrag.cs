using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Storage _storage;
    UISlot _slot;
    GameObject _dragInstance;
    
    
    public void SetupStorage(Storage storage, UISlot slot)
    {
        _storage = storage;
        _slot = slot;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_storage.StaticStorage)
        {
            return;
        }
        
        _storage.SwapItem(_slot);

        _dragInstance = new GameObject("Drag " + _slot.name);
        var image = _dragInstance.AddComponent<Image>();
        
        image.sprite = _slot.ItemImage.sprite;
        image.raycastTarget = false;
        
        _dragInstance.transform.SetParent(_storage.transform);
        _dragInstance.transform.localScale = Vector3.one;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_dragInstance != null)
        {
            _dragInstance.transform.position = InputManager.Instance.PlayerInputActions.Player.Aiming.ReadValue<Vector2>();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject is GameObject targetObj)
        {
            var targetSlot = targetObj.GetComponentInParent<UISlot>();
            if (targetObj != null)
            {
                _storage.SwapItem(targetSlot);
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        _storage.ClearSwap();

        if (_dragInstance != null)
        {
            Destroy(_dragInstance);
        }
    }
}

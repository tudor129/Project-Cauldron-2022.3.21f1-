using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Serialization;

public class FloatEffect : MonoBehaviour
{
    [SerializeField] float _floatHeight = 0.5f;
    [SerializeField] float _floatDuration = 1f;
    [SerializeField] float _rotationDuration = 1f;

    Vector3 _startPos = Vector3.up;
    Collider _collider;

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    void Start()
    {
        _startPos = transform.position;
        StartFloating();
        StartRotating();
    }

    void StartFloating()
    {
        transform.DOMoveY(_startPos.y + _floatHeight, _floatDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DOMoveY(_startPos.y, _floatDuration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                StartFloating();
            });
        });
    }
    void StartRotating()
    {
        if (_collider != null)
        {
            // Create an empty GameObject and set its position to the center of your loot object
            GameObject rotationCenter = new GameObject("Rotation Center");
            if (rotationCenter != null)
            {
                rotationCenter.transform.position = _collider.bounds.center; 
                
            }

            // Make your loot object a child of the empty GameObject
            transform.parent = rotationCenter.transform;

            // Apply the rotation tween to the parent GameObject
            rotationCenter.transform.DORotate(new Vector3(0f, 360f, 0f), _rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

    }
}

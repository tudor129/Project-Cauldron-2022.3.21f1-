using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine.Serialization;

public class FloatEffect : MonoBehaviour
{
    [SerializeField] float _floatHeight = 0.5f;
    [SerializeField] float _floatDuration = 1f;
    [SerializeField] float _rotationDuration = 1f;

    Vector3 _startPos = Vector3.up;
    Collider _collider;
    Tween  _rotationTween;

    void Awake()
    {
        _collider = GetComponent<Collider>();
        DOTween.Init();
    }

    void OnEnable()
    {
        DOTween.Init();
        StartFloating();
        StartRotating();
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
        // Ensure only one rotation tween is active at a time
        if (_rotationTween != null && _rotationTween.IsActive())
        {
            _rotationTween.Kill();
        }

        // Apply rotation directly to the object
        _rotationTween = transform.DORotate(new Vector3(360f, 0f, 0f), _rotationDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}
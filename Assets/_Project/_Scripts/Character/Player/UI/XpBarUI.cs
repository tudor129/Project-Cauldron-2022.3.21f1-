using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class XpBarUI : MonoBehaviour
{
    [SerializeField] Slider _slider;
    
    Tween _xpTween;

    
    bool callSetXpAfterLevelUp = false;

    void Awake()
    {
        if (_slider == null)
        {
            _slider = GetComponent<Slider>();
        }
    }

    void Start()
    {
        EventManager<int>.TriggerEvent(EventKey.UPDATE_EXPERIENCE_AFTER_LEVEL_UP, 0);
    }

    void OnEnable()
    {
        EventManager<int>.RegisterEvent(EventKey.UPDATE_EXPERIENCE, SetCurrentXp);
        EventManager<int>.RegisterEvent(EventKey.UPDATE_MAX_EXPERIENCE, SetMaxXp);
        EventManager<int>.RegisterEvent(EventKey.UPDATE_EXPERIENCE_AFTER_LEVEL_UP, SetXpAfterLevelUp);
        EventManager<EventArgs>.RegisterEvent(EventKey.FINISH_XP_BAR_TWEEN, FinishXpBarTween);
    }
    void OnDisable()
    {
        EventManager<int>.UnregisterEvent(EventKey.UPDATE_EXPERIENCE, SetCurrentXp);
        EventManager<int>.UnregisterEvent(EventKey.UPDATE_MAX_EXPERIENCE, SetMaxXp);
        EventManager<int>.UnregisterEvent(EventKey.UPDATE_EXPERIENCE_AFTER_LEVEL_UP, SetXpAfterLevelUp);
        EventManager<EventArgs>.UnregisterEvent(EventKey.FINISH_XP_BAR_TWEEN, FinishXpBarTween);
    }


    void SetMaxXp(int maxXp)
    {
        //_slider.DOKill(true);
        _slider.maxValue = maxXp;
    }
    void SetCurrentXp(int currentXp)
    {
        //_slider.value += currentXp;
        _slider.DOKill(true);
        
        
        
        float newTargetXp = Mathf.Clamp(_slider.value + currentXp, 0, _slider.maxValue);

        _xpTween = _slider.DOValue(newTargetXp, 0.8f).SetEase(Ease.OutCubic);
    }
    void SetXpAfterLevelUp(int currentXp)
    {
        _slider.DOKill(true);
       // if tween is active return
        if (_xpTween != null && _xpTween.IsActive())
        {
            return;
        }
        int currentOffset = Mathf.RoundToInt(_slider.value - _slider.minValue);
        int maxOffset = Mathf.RoundToInt(_slider.maxValue - _slider.minValue);
        _slider.value = currentOffset / maxOffset;
        //_slider.value = currentXp;
        
    }
    
    void FinishXpBarTween(EventArgs args)
    {
        _slider.DOKill(true);
        _xpTween = _slider.DOValue(_slider.maxValue, 0.5f).SetEase(Ease.OutCubic);
    }
    
   
   
}

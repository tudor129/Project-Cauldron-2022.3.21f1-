using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBallUI : MonoBehaviour
{
    [SerializeField] Image _backgroundFillImage;
    
    Image _fillImage;
    void Awake()
    {
        _fillImage = GetComponent<Image>();
    }
    public void SetHealthBallAmount(float amount)
    {
        if (_fillImage && _backgroundFillImage != null)
        {
            _fillImage.fillAmount = amount;
            _backgroundFillImage.fillAmount = amount;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBallUI : MonoBehaviour
{
    [SerializeField] Image _backgroundFillImage;

    Image _fillImage;
    void Awake()
    {
        _fillImage = GetComponent<Image>();
       
    }
   
    
    public void SetMaxMana(float maxMana)
    {
        _fillImage.fillAmount = maxMana;
        _backgroundFillImage.fillAmount = maxMana;
    }
    
    public void SetCurrentMana(float currentMana, float maxMana)
    {
        float normalizedMana = currentMana / maxMana;
        _fillImage.fillAmount = normalizedMana;
        _backgroundFillImage.fillAmount = normalizedMana;
    }
}

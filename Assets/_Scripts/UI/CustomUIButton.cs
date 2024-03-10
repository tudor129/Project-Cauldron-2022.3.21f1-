using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomUIButton : CustomUIComponent
{
    public ThemeSO _theme;
    public Style _style;
    public UnityEvent _onClick;

    Button _button;
    TextMeshProUGUI _buttonText;
    
    public override void Setup()
    {
        _button = GetComponentInChildren<Button>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public override void Configure()
    {
        ColorBlock cb = _button.colors;
        cb.normalColor = _theme.GetBackgroundColor(_style);
        _button.colors = cb;
        
        _buttonText.color = _theme.GetTextColor(_style);
        _buttonText.alpha = 100;
    }
    
    public void OnClick()
    {
        _onClick.Invoke();
    }
}

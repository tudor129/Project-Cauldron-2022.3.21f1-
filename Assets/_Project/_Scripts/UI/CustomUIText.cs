using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

public class CustomUIText : CustomUIComponent
{
    public CustomUITextSO _textData;
    public Style _style;

    TextMeshProUGUI _textMeshProUGUI;

    public override void Setup()
    {
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Configure()
    {
         _textMeshProUGUI.color = _textData._theme.GetTextColor(_style);
         _textMeshProUGUI.font = _textData._font;
         _textMeshProUGUI.fontSize = _textData._size;
         _textMeshProUGUI.alpha = 100;
    }
   
}

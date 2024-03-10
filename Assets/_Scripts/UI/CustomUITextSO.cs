using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomUI/CustomUITextSO", fileName = "CustomUITextSO")]
public class CustomUITextSO : SerializedScriptableObject
{
    public ThemeSO _theme;
    public TMP_FontAsset _font;
    public float _size;
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomUI/ViewSO", fileName = "ViewSO")]
public class ViewSO : SerializedScriptableObject
{
    public ThemeSO _theme;
    public RectOffset _padding;
    public float _spacing;
}

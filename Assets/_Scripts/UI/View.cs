using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using Sirenix.OdinInspector;

public class View : CustomUIComponent
{
    public ViewSO _viewData;

    public GameObject _containerTop;
    public GameObject _containerCenter;
    public GameObject _containerBottom;

    Image _imageTop;
    Image _imageCenter;
    Image _imageBottom;

    VerticalLayoutGroup _verticalLayoutGroup;
   
    public override void Setup()
    {
        _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        _imageTop = _containerTop.GetComponent<Image>();
        _imageCenter = _containerCenter.GetComponent<Image>();
        _imageBottom = _containerBottom.GetComponent<Image>();
    }

    public override void Configure()
    {
        _verticalLayoutGroup.padding = _viewData._padding;
        _verticalLayoutGroup.spacing = _viewData._spacing;

        _imageTop.color = _viewData._theme._primary_bg;
        _imageCenter.color = _viewData._theme._secondary_bg;
        _imageBottom.color = _viewData._theme._primary_bg;
    }

   

}

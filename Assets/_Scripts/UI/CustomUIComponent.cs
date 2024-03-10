using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class CustomUIComponent : MonoBehaviour
{
    void Awake()
    {
        Init();
    }
    [Button("Configure Now")]
    void Init()
    {
        Setup();
        Configure();
    }
    public abstract void Configure();

    public abstract void Setup();

    void OnValidate()
    {
        Init();
    }



}

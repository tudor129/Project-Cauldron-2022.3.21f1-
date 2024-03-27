using System.Collections.Generic;
using UnityEngine;

public class OpacityChanger : MonoBehaviour
{
    public List<Material> materials;
    public float opacity = 0.5f;

    void Start()
    {
        ChangeOpacity(opacity);
    }

    public void ChangeOpacity(float newOpacity)
    {
        foreach (Material material in materials)
        {
            Color baseColor = material.GetColor("_BaseColor");
            baseColor.a = Mathf.Clamp01(newOpacity);
            material.SetColor("_BaseColor", baseColor);
        }
    }
}

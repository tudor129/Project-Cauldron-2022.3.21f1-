using System;
using UnityEngine;
public class SetCoinRotation : MonoBehaviour
{
    void Start()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStruckEffect : MonoBehaviour
{
   void Start()
   {
      Destroy(gameObject, 5f);
   }
}

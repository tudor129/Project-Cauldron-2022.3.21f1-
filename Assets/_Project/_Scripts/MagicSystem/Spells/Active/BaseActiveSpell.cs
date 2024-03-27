using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseActiveSpell : Spell
{
    protected override void Awake()
    {
        base.Awake();
        
        
    }

    protected override void Start()
    {
        base.Start();
        // if (spellData.HasFlash)
        // {
        //     HandleFlashEffect();
        // }
    }
   

    protected virtual void FixedUpdate()
    {
      
    }
}

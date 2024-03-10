using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MagicFlameVisual : CastingEffect
{

    // Update is called once per frame
    protected override void Update()
    {
        transform.position = GameManager.Instance.player.transform.position;
        transform.forward = GameManager.Instance.player.transform.forward;
    }
}

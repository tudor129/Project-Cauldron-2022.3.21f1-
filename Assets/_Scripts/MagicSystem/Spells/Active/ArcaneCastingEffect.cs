using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneCastingEffect : CastingEffect
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        transform.position = GameManager.Instance._playerCastPoint.transform.position;
    }
}

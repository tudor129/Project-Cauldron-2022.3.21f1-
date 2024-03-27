using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCastingEffect : CastingEffect
{
    void Start()
    {
       
    }

    protected override void Update()
    {
        transform.position = GameManager.Instance.player.transform.position + new Vector3(0, 0.01f, 0);
    }
    
    protected override IEnumerator DestroyAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Instantiate another object after the duration

        // Destroy the current object
        Destroy(gameObject);
    }
    
   
   
    
    
}

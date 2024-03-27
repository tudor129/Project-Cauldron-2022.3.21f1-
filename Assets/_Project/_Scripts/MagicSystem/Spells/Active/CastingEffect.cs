using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CastingEffect : MonoBehaviour
{
    void Start()
    {
            // _flashInstance = GetComponent<ParticleSystem>();
            // if (_flashInstance != null)
            // {
            //     StartCoroutine(DestroyAfterDuration(_flashInstance.main.duration));
            // }
    }

    protected virtual void Update()
    {
        transform.position = GameManager.Instance._playerCastPoint.transform.position + new Vector3(0, 0.01f, 0);
    }

    protected virtual IEnumerator DestroyAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Instantiate another object after the duration
        //_fireImpactInstance = Instantiate(Projectile, transform.position, transform.rotation);

        // Destroy the current object
        Destroy(gameObject);
    }
}

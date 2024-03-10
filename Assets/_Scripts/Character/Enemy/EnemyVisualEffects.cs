using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVisualEffects : MonoBehaviour
{
    [SerializeField] Material _enemyHitMaterial;
    [SerializeField] Renderer _renderer;

    Material _originalMaterial; 
    
    void Awake()
    {
        _originalMaterial = _renderer.material;
    }

    public void HandleMaterialSwap()
    {
        // Swap the material to the hit material
        _renderer.material = _enemyHitMaterial;
        // Start a coroutine to swap back to the original material after a delay
        StartCoroutine(SwapMaterialBackAfterDelay(0.1f));
    }
    
    IEnumerator SwapMaterialBackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _renderer.material = _originalMaterial;
    }
    
    public void HandleSpellEffect(Spell.Stats spellInfo)
    {
        if (spellInfo.IsFire)
        {
            

            bool effectExists = false;
            if (spellInfo.CharacterEffect != null)
            {
                foreach (Transform child in transform)
                {
                    if (child.name == spellInfo.CharacterEffect.name + "(Clone)") //  the instantiated object has to have "(Clone)" in its name
                    {
                        effectExists = true;
                        break;
                    }
                }
            }
            if (!effectExists && spellInfo.CharacterEffect != null && spellInfo.DoesDoT)
            {
                GameObject fireEffect = Instantiate(spellInfo.CharacterEffect, transform.position, transform.rotation);
                fireEffect.transform.SetParent(transform);
                VisualEffect visualEffect = fireEffect.GetComponent<VisualEffect>();

                if (visualEffect != null)
                {
                    SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

                    if (skinnedMeshRenderers.Length > 1)
                    {
                        SkinnedMeshRenderer targetRenderer = skinnedMeshRenderers[1];
                        visualEffect.SetSkinnedMeshRenderer("SkinnedMeshRenderer", targetRenderer);
                    }
                }
                    
                Destroy(fireEffect, spellInfo.DamageOverTimeDuration + spellInfo.DamageOverTimeInitialDelay + 0.4f);
            }
        }
    }
}

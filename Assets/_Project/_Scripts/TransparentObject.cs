using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [SerializeField] float _fadeSpeed = 1f;
    [SerializeField] TransparentObjectsController _transparentObjectsController;
    
    Dictionary<GameObject, Coroutine> _fadeCoroutines = new Dictionary<GameObject, Coroutine>();

    void Start()
    {
        if (_transparentObjectsController != null)
        {
            _transparentObjectsController.OnObjectInTheWay += TransparentObjectsController_OnObjectInTheWay;
            _transparentObjectsController.OnObjectNotInTheWay += TransparentObjectsController_OnObjectNotInTheWay;
        }
    }

    void OnDestroy()
    {
        if (_transparentObjectsController != null)
        {
            _transparentObjectsController.OnObjectInTheWay -= TransparentObjectsController_OnObjectInTheWay;
            _transparentObjectsController.OnObjectNotInTheWay -= TransparentObjectsController_OnObjectNotInTheWay;
        }
    }
    
    void TransparentObjectsController_OnObjectInTheWay(GameObject blockingObject)
    {
        Coroutine fadeCoroutine;
        if (_fadeCoroutines.TryGetValue(blockingObject, out fadeCoroutine))
        {
            if(fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
        }
        fadeCoroutine = StartCoroutine(FadeOpacity(blockingObject, 1f));
        _fadeCoroutines[blockingObject] = fadeCoroutine;
    }
    
    void TransparentObjectsController_OnObjectNotInTheWay(GameObject lastBlockingObject)
    {
        Coroutine fadeCoroutine;
        if (_fadeCoroutines.TryGetValue(lastBlockingObject, out fadeCoroutine))
        {
            if(fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
        }
        fadeCoroutine = StartCoroutine(FadeOpacity(lastBlockingObject, 0.2f));
        _fadeCoroutines[lastBlockingObject] = fadeCoroutine;
    }


    IEnumerator FadeOpacity(GameObject currentFadingBlockingObject, float targetOpacity)
    {
        Renderer[] renderers = currentFadingBlockingObject.GetComponentsInChildren<Renderer>();
    
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
        
            foreach (Material material in materials)
            {
                if (material.HasProperty("_AlphaCutoff"))
                {
                    float currentOpacity = material.GetFloat("_AlphaCutoff");
                    float startTime = Time.time;

                    while (Time.time < startTime + _fadeSpeed)
                    {
                        float progress = (Time.time - startTime) / _fadeSpeed;
                        float newOpacity = Mathf.Lerp(currentOpacity, targetOpacity, progress);
                        material.SetFloat("_AlphaCutoff", newOpacity);
                        yield return null;
                    }

                    // Make sure the final opacity value is set correctly
                    material.SetFloat("_AlphaCutoff", targetOpacity);
                }
            }
        }

        _fadeCoroutines.Remove(currentFadingBlockingObject);
    }

}

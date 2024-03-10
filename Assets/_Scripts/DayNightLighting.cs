using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class DayNightLighting : MonoBehaviour
{
    Light _light;
    
    [SerializeField] float _dayIntensity = 0f;
    [SerializeField] float _nightIntensity = 1f;
    [SerializeField] float _transitionDuration = 3f;

    void Awake()
    {
        _light = GetComponent<Light>();
    }

    void Start()
    {
        TimeController.Instance.OnSunrise += OnSunrise;
        TimeController.Instance.OnSunset += OnSunset;
        HandleInitializationComplete();

    }

    void OnDestroy()
    {
        TimeController.Instance.OnSunrise -= OnSunrise;
        TimeController.Instance.OnSunset -= OnSunset;
    }

    void HandleInitializationComplete()
    {
        if (TimeController.Instance.IsDayTime())
        {
            _light.intensity = _dayIntensity;
            gameObject.SetActive(true);
        }
        else if (TimeController.Instance.IsNightTime())
        {
            _light.intensity = _nightIntensity;
            gameObject.SetActive(true);
        }
    }
    void OnSunset()
    {
        gameObject.SetActive(true);
        CoroutineManager.Instance.StartManagedCoroutine(TransitionToIntensity(_nightIntensity));
    }
  
    void OnSunrise()
    {
        CoroutineManager.Instance.StartManagedCoroutine(TransitionToIntensity(_dayIntensity));
    }
    
    IEnumerator TransitionToIntensity(float targetIntensity)
    {
        float originalIntensity = _light.intensity;
        float duration = 5f; // seconds
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (_light == null)
            {
                yield break;
            }
            _light.intensity = Mathf.Lerp(originalIntensity, targetIntensity, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _light.intensity = targetIntensity;

        // Disable the gameObject after the transition is done
        if (targetIntensity == 0f)
        {
            gameObject.SetActive(false);
        }
    }
    
}

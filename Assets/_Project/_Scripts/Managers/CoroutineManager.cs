using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour 
{
    static CoroutineManager _instance;

    public static CoroutineManager Instance
    {
        get 
        {
            if (_instance == null)
            {
                var go = new GameObject("[CoroutineManager]");
                _instance = go.AddComponent<CoroutineManager>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }
    
    public void StartManagedCoroutine(IEnumerator coroutine, System.Action onComplete = null) 
    {
        StartCoroutine(RunManagedCoroutine(coroutine, onComplete));
    }
    
    IEnumerator RunManagedCoroutine(IEnumerator coroutine, System.Action onComplete = null)
    {
        yield return StartCoroutine(coroutine);

        onComplete?.Invoke();
    }
    
    
}

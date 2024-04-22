using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour 
{
    static CoroutineManager _instance;

    Dictionary<GameObject, List<Coroutine>> _activeCoroutines = new Dictionary<GameObject, List<Coroutine>>();
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
        //StartCoroutine(RunManagedCoroutine(coroutine, onComplete));
        /*Coroutine newCoroutine = null;
        newCoroutine = StartCoroutine(RunManagedCoroutine(coroutine, () =>
        {
            onComplete?.Invoke();
            // Remove coroutine from the tracking dictionary when completed
            if (_activeCoroutines.ContainsKey(owner))
            {
                _activeCoroutines[owner].Remove(newCoroutine);
                if (_activeCoroutines[owner].Count == 0)
                {
                    _activeCoroutines.Remove(owner);
                }
            }
        }));
        
        // Add to dictionary
        if (!_activeCoroutines.ContainsKey(owner))
        {
            _activeCoroutines[owner] = new List<Coroutine>();
        }
        _activeCoroutines[owner].Add(newCoroutine);*/
    }
    
    IEnumerator RunManagedCoroutine(IEnumerator coroutine, System.Action onComplete = null)
    {
        yield return StartCoroutine(coroutine);

        onComplete?.Invoke();
    }
    
    
}

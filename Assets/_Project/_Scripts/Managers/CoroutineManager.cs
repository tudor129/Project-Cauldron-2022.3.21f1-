using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour 
{
    static CoroutineManager _instance;

    [ShowInInspector]
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
        StartCoroutine(RunManagedCoroutine(coroutine, onComplete));
    }
    
   
    
    public void StartManagedCoroutine(GameObject owner, IEnumerator coroutine, System.Action onComplete = null) 
    {
        Coroutine newCoroutine = StartCoroutine(RunManagedCoroutine(coroutine, onComplete));
        RegisterCoroutine(owner, newCoroutine);
    }
    
    void RegisterCoroutine(GameObject owner, Coroutine coroutine)
    {
        if (!_activeCoroutines.ContainsKey(owner))
        {
            _activeCoroutines[owner] = new List<Coroutine>();
        }
        _activeCoroutines[owner].Add(coroutine);
        Debug.Log("Registered coroutine to " + owner.name);
        Debug.Log("Active coroutines: " + _activeCoroutines[owner].Count);

        // Setting up the onComplete within this method ensures coroutine is already registered and valid
        StartCoroutine(CleanupCoroutine(owner, coroutine));
    }
    
    IEnumerator CleanupCoroutine(GameObject owner, Coroutine coroutine)
    {
        yield return coroutine; // Wait for the coroutine to complete
        RemoveCoroutineFromOwner(owner, coroutine);
    }

    IEnumerator RunManagedCoroutine(IEnumerator coroutine, System.Action onComplete = null)
    {
        yield return StartCoroutine(coroutine);

        onComplete?.Invoke();
    }
    
    void RemoveCoroutineFromOwner(GameObject owner, Coroutine coroutine)
    {
        if (_activeCoroutines.ContainsKey(owner))
        {
            _activeCoroutines[owner].Remove(coroutine);
            if (_activeCoroutines[owner].Count == 0)
            {
                _activeCoroutines.Remove(owner);
            }
        }
    }
    
    public void StopAllCoroutinesOwnedBy(GameObject owner)
    {
        if (_activeCoroutines.ContainsKey(owner))
        {
            foreach (Coroutine coroutine in _activeCoroutines[owner])
            {
                StopCoroutine(coroutine);
            }
            _activeCoroutines.Remove(owner);
            Debug.Log("Stopped all coroutines owned by " + owner.name);
        }
    }
    
   
    
    
}

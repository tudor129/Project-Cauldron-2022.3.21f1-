using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;
public class FireBehavior : BaseSpellBehavior
{
    [SerializeField] GameObject _firePrefab;
    
    Camera _mainCamera;
    
    protected override void Start()
    {
        // _mainCamera.depth = Camera.main.depth + 1;
        // _mainCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        //GUI.Button(new Rect(10, 10, 150, 100), "Table");
        //Searcher.AnalyticsEvent.
        
        // SendMessageUpwards("OnSpellCast", _currentStats, SendMessageOptions.DontRequireReceiver);
        // BroadcastMessage("OnSpellCast", _currentStats, SendMessageOptions.DontRequireReceiver);
    }
    

    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            bool effectExists = other.gameObject.transform.Find(_firePrefab.name);
            if (effectExists)
            {
                return;
            }
            other.GetComponent<EnemyHealth>().ApplyDoTEffect(_currentStats);
            // GameObject lavaObject = ObjectPoolManager.Instance.SpawnObject(
            //     _firePrefab,
            //     other.transform.position + new Vector3(0, 0.1f, 0),
            //     Quaternion.identity,
            //     ObjectPoolManager.PoolType.StatusEffects);
            //
            // CoroutineManager.Instance.StartManagedCoroutine(ReturnToPoolAfterDelay(10f, lavaObject));
            
            GameObject fireObject = ObjectPoolManager.Instance.SpawnStatusEffect(
                _firePrefab,
                other.gameObject,
                other.transform.position + new Vector3(0, 0.1f, 0),
                Quaternion.identity,
                ObjectPoolManager.PoolType.StatusEffects);
            
            CoroutineManager.Instance.StartManagedCoroutine(ReturnStatusEffectToPoolAfterDelay(10f, fireObject));
        }
    }
    
    IEnumerator ReturnToPoolAfterDelay(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);
        ObjectPoolManager.Instance.ReturnObjectToPool(objectToReturn);
    }
    
    IEnumerator ReturnStatusEffectToPoolAfterDelay(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);
        ObjectPoolManager.Instance.ReturnStatusEffectToPool(objectToReturn);
    }
    
    
}

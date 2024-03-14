using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    static ObjectPoolManager _poolManager;

    void Awake()
    {
         
    }

    void Start()
    {
        //StartCoroutine(ReturnToPoolAfterDelay(5f));
    }
    
    IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // wait for 'delay' seconds
        ReturnGameObjectToPool(); // then call your function to return the game object to the pool
    }

    public void ReturnGameObjectToPool()
    {
        if (gameObject.activeInHierarchy && gameObject != null)
        {
            ObjectPoolManager.Instance.ReturnEnemyObjectToPool(gameObject);
        }
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public enum PoolType
    {
        ImpactEffects,
        GameObject,
        None,
        Enemy, 
        Projectiles,
        Decals,
        StatusEffects
    }
    
   public static ObjectPoolManager Instance { get; private set; }
   public List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

   GameObject _objectPoolEmptyHolder;

   static GameObject _impactEffectEmpty;
   static GameObject _gameObjectEmpty;
   static GameObject _enemyEmpty;
   static GameObject _projectilesEmpty;
   static GameObject _decalsEmpty;
   static GameObject _statusEffectsEmpty;
   
   public CustomObjectPool<GameObject> _impactEffectPool;
   public CustomObjectPool<GameObject> _enemyPool; 
   public CustomObjectPool<GameObject> _projectilePool;
   public CustomObjectPool<GameObject> _gameObjectPool;
   public CustomObjectPool<GameObject> _statusEffectsPool;
   public CustomObjectPool<GameObject> _decalsPool;
   
   void Awake()
   {
       if (Instance == null)
       {
           Instance = this;
           DontDestroyOnLoad(gameObject);
       }
       else
       {
           Destroy(gameObject);
       }
       
       SetupEmpties();
       SetupPools();
    
   }
   
   GameObject CreatePooledObject(GameObject objToSpawn, Vector3 position, PoolType poolType)
   {
       GameObject obj = Instantiate(objToSpawn, position, Quaternion.identity);
       GameObject parent = SetParentObject(poolType);
       obj.transform.SetParent(parent.transform, true);
       return obj.gameObject;
   }
    
   void OnTakeFromPool(GameObject projectile)
   {
       projectile.gameObject.SetActive(true);
   }
    
   public void OnReturnToPool(GameObject projectile)
   {
       projectile.gameObject.SetActive(false);
   }
    
   void OnDestroyObject(GameObject projectile)
   {
       Destroy(projectile.gameObject);
   }
    
   
    void SetupEmpties()
    {
         _objectPoolEmptyHolder = new GameObject("ObjectPoolEmptyHolder");
         _projectilesEmpty = new GameObject("ProjectilesEmpty");
         _impactEffectEmpty = new GameObject("ImpactEffectEmpty");
         _gameObjectEmpty = new GameObject("GameObjectEmpty");
         _enemyEmpty = new GameObject("EnemyEmpty");
         _decalsEmpty = new GameObject("DecalsEmpty");
         _statusEffectsEmpty = new GameObject("StatusEffectsEmpty");
         _projectilesEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         _impactEffectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         _gameObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         _enemyEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         _decalsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         _statusEffectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }
    void SetupPools()
    {
        _impactEffectPool = new CustomObjectPool<GameObject>(
            ObjectPoolManager.Instance.CreatePooledObject, 
            ObjectPoolManager.Instance.OnTakeFromPool, 
            ObjectPoolManager.Instance.OnReturnToPool, 
            ObjectPoolManager.Instance.OnDestroyObject, 
            false,
            200,
            500);
        _enemyPool = new CustomObjectPool<GameObject>(
            ObjectPoolManager.Instance.CreatePooledObject,
            ObjectPoolManager.Instance.OnTakeFromPool, 
            ObjectPoolManager.Instance.OnReturnToPool, 
            ObjectPoolManager.Instance.OnDestroyObject, 
            false,
            200,
            500);
        _projectilePool = new CustomObjectPool<GameObject>(
            ObjectPoolManager.Instance.CreatePooledObject, 
            ObjectPoolManager.Instance.OnTakeFromPool, 
            ObjectPoolManager.Instance.OnReturnToPool, 
            ObjectPoolManager.Instance.OnDestroyObject, 
            false,
            200,
            500);
        _gameObjectPool = new CustomObjectPool<GameObject>(
            ObjectPoolManager.Instance.CreatePooledObject, 
            ObjectPoolManager.Instance.OnTakeFromPool, 
            ObjectPoolManager.Instance.OnReturnToPool, 
            ObjectPoolManager.Instance.OnDestroyObject, 
            false,
            200,
            500);
        _statusEffectsPool = new CustomObjectPool<GameObject>(
            ObjectPoolManager.Instance.CreatePooledObject, 
            ObjectPoolManager.Instance.OnTakeFromPool, 
            ObjectPoolManager.Instance.OnReturnToPool, 
            ObjectPoolManager.Instance.OnDestroyObject, 
            false,
            200,
            500);
        _decalsPool = new CustomObjectPool<GameObject>(
            ObjectPoolManager.Instance.CreatePooledObject, 
            ObjectPoolManager.Instance.OnTakeFromPool, 
            ObjectPoolManager.Instance.OnReturnToPool, 
            ObjectPoolManager.Instance.OnDestroyObject, 
            false,
            200,
            500);
        
    }
    
    
   public T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None) where T : Component
   {
       PooledObjectInfo pool = null;
       // Use objectToSpawn.gameObject.name to access the name property
       string objectName = objectToSpawn.gameObject.name;

       foreach (PooledObjectInfo p in ObjectPools)
       {
           if (p.LookupString == objectName)
           {
               pool = p;
               break;
           }
       }
       
       // If the pool doesn't exist, create it
       if (pool == null)
       {
           pool = new PooledObjectInfo()
           {
               LookupString = objectName
           };

           ObjectPools.Add(pool);
       }
       
       GameObject spawnableObject = null;
       foreach (GameObject obj in pool.InactiveObjects)
       {
           if (obj != null)
           {
               spawnableObject = obj;
               break;
           }
       }
       
       if (spawnableObject == null)
       {
           // Find the parent of the empty object
           GameObject parentObject = SetParentObject(poolType);

           // If there are no inactive objects, instantiate a new one
           spawnableObject = Instantiate(objectToSpawn.gameObject, spawnPosition, spawnRotation);
           spawnableObject.name = objectName; 
           if (parentObject != null)
           {
               spawnableObject.transform.SetParent(parentObject.transform);
           }
       }
       else
       {
           // If there is an inactive object, reactivate it
           spawnableObject.transform.position = spawnPosition;
           spawnableObject.transform.rotation = spawnRotation;
           pool.InactiveObjects.Remove(spawnableObject);
           spawnableObject.SetActive(true);
       }
       
       // Ensure the component of type T is attached to the spawnableObject
       T component = spawnableObject.GetComponent<T>();
       if (component == null)
       {
           // If the required component is not found, add it
           component = spawnableObject.AddComponent<T>();
       }

       return component; // This ensures the method returns the correct type
   }
    
   public GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
   {
       PooledObjectInfo pool = null;
       foreach (PooledObjectInfo p in ObjectPools)
       {
           if (p.LookupString == objectToSpawn.name)
           {
               pool = p;
               break;
           }
       }
       // If the pool doesn't exist, create it
       if (pool == null)
       {
           pool = new PooledObjectInfo()
           {
               LookupString = objectToSpawn.name
               
           };
           
              ObjectPools.Add(pool);
       }
       // Check if there are any inactive objects in the pool
       GameObject spawnableObject = null;
       foreach (GameObject obj in pool.InactiveObjects)
       {
           if (obj != null)
           {
               spawnableObject = obj;
               break;
           }
       }

       if (spawnableObject == null)
       {
           //Find the parent of the empty object
           GameObject parentObject = SetParentObject(poolType);
           
           
           // If there are no inactive objects, create a new one
           spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
           spawnableObject.name = objectToSpawn.name;
           
           if (parentObject != null)
           {
               spawnableObject.transform.SetParent(parentObject.transform);
           }
       }
       else
       {
           // If there is an inactive object, reactivate it
           spawnableObject.transform.position = spawnPosition;
           spawnableObject.transform.rotation = spawnRotation;
           pool.InactiveObjects.Remove(spawnableObject);
           spawnableObject.SetActive(true);
       }
       
       if (poolType == PoolType.Enemy)
       {
           // Set player transform reference
           Enemy enemyScript = spawnableObject.GetComponent<Enemy>();
           enemyScript.SetPlayerTransformFromPool(GameManager.Instance.GetPlayerTransform());
           Health enemyHealth = spawnableObject.GetComponent<Health>();
           //enemyHealth.Respawn();
       }
       return spawnableObject;
   }
   
   public GameObject SpawnStatusEffect(GameObject objectToSpawn, GameObject parent, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
   {
       PooledObjectInfo pool = null;
       foreach (PooledObjectInfo p in ObjectPools)
       {
           if (p.LookupString == objectToSpawn.name)
           {
               pool = p;
               break;
           }
       }
       // If the pool doesn't exist, create it
       if (pool == null)
       {
           //GameObject poolHolder = new GameObject(_statusEffectsEmpty + " Pool");
           pool = new PooledObjectInfo()
           {
               LookupString = objectToSpawn.name,
               ParentHolder = _statusEffectsEmpty
               
           };
           
              ObjectPools.Add(pool);
       }
       // Check if there are any inactive objects in the pool
       GameObject spawnableObject = null;
       foreach (GameObject obj in pool.InactiveObjects)
       {
           if (obj != null)
           {
               spawnableObject = obj;
               break;
           }
       }

       if (spawnableObject == null)
       {
           //Find the parent of the empty object
           GameObject parentObject = SetParentObject(poolType);
           
           // If there are no inactive objects, create a new one
           spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
           spawnableObject.name = objectToSpawn.name;
           
           // if (parentObject != null)
           // {
           //     spawnableObject.transform.SetParent(parent.transform);
           // }
       }
       else
       {
           // If there is an inactive object, reactivate it
           spawnableObject.transform.position = spawnPosition;
           spawnableObject.transform.rotation = spawnRotation;
           pool.InactiveObjects.Remove(spawnableObject);
           spawnableObject.SetActive(true);
       }
       
       if (parent != null)
       {
           spawnableObject.transform.SetParent(parent.transform);
       }
       //spawnableObject.transform.SetParent(parent.transform);
       
       return spawnableObject;
   }
   
   public void ReturnParentObjectToPool(GameObject obj)
   {
       GameObject parentObject = obj.transform.parent.gameObject; // Get the immediate parent object
       
       string goName = parentObject.name;
       
       if (goName.Contains("(Clone)"))
       {
           goName = goName.Replace("(Clone)", ""); // Replace "(Clone)" with an empty string
       }

       PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
   
       if (pool == null)
       {
           Debug.LogError("No pool exists for " + goName);
           return;
       }
       else
       {
           parentObject.SetActive(false);
           pool.InactiveObjects.Add(parentObject);
       }
   }
   
   public void ReturnObjectToPool(GameObject obj)
   {
       
       string goName = obj.name;
       
       if (goName.Contains("(Clone)"))
       {
           goName = goName.Replace("(Clone)", ""); // Replace "(Clone)" with an empty string
       }

       PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
   
       if (pool == null)
       {
           Debug.LogError("No pool exists for " + goName);
           return;
       }
       else
       {
           if (!obj.activeSelf)
           {
               Debug.Log("Object " + obj.name + " is already inactive.");
               return;
           }
           obj.SetActive(false);
           pool.InactiveObjects.Add(obj);
           //Debug.Log("Object " + obj.name + " returned to pool." + obj.activeSelf);
       }
   }
   
   public void ReturnStatusEffectToPool(GameObject obj)
   {
       string goName = obj.name;
    
       if (goName.Contains("(Clone)"))
       {
           goName = goName.Replace("(Clone)", "");
       }

       PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

       if (pool == null)
       {
           Debug.LogError("No pool exists for " + goName);
           return;
       }
       else
       {
           obj.SetActive(false);
           obj.transform.SetParent(pool.ParentHolder.transform); // Re-parent to the pool holder
           pool.InactiveObjects.Add(obj);
       }
   }
   
   static GameObject SetParentObject(PoolType poolType)
   {
       switch (poolType)
       {
           case PoolType.ImpactEffects:
               return _impactEffectEmpty;
           case PoolType.GameObject:
               return _gameObjectEmpty;
           case PoolType.None:
               return null;
           case PoolType.Enemy:
               return _enemyEmpty;
           case PoolType.Projectiles:
               return _projectilesEmpty;
           case PoolType.Decals:
               return _decalsEmpty;
           case PoolType.StatusEffects:
               return _statusEffectsEmpty;
           default:
               return null;
       }
   }
   
  
   
}

[System.Serializable]
public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
    public GameObject ParentHolder;
}

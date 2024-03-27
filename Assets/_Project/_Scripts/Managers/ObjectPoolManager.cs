using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
   public static ObjectPoolManager Instance { get; private set; }
   public  List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
  

   GameObject _objectPoolEmptyHolder;

   static GameObject _impactHitsEmpty;
   static GameObject _gameObjectEmpty;
   static GameObject _enemyEmpty;
   static GameObject _projectilesEmpty;
  

   public enum PoolType
   {
       ImpactHits,
       GameObject,
       None,
       Enemy, 
       Projectiles
   }

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
   }
   
    void SetupEmpties()
    {
         _objectPoolEmptyHolder = new GameObject("ObjectPoolEmptyHolder");
         _projectilesEmpty = new GameObject("ProjectilesEmpty");
         _impactHitsEmpty = new GameObject("ImpactHitsEmpty");
         _gameObjectEmpty = new GameObject("GameObjectEmpty");
         _enemyEmpty = new GameObject("EnemyEmpty");
         _projectilesEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         _impactHitsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         _gameObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         _enemyEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
         
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
   
   public ProjectileBehavior SpawnObject(ProjectileBehavior objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
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
           spawnableObject = Instantiate(objectToSpawn.gameObject, spawnPosition, spawnRotation);
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
           pool.InactiveObjects.Remove(spawnableObject.gameObject);
           spawnableObject.gameObject.SetActive(true);
       }
       
       return spawnableObject.gameObject.GetComponent<ProjectileBehavior>();
   }
   
   public SpellImpactBehavior SpawnObject(SpellImpactBehavior objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
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
           spawnableObject = Instantiate(objectToSpawn.gameObject, spawnPosition, spawnRotation);
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
           pool.InactiveObjects.Remove(spawnableObject.gameObject);
           spawnableObject.gameObject.SetActive(true);
       }
       
       return spawnableObject.gameObject.GetComponent<SpellImpactBehavior>();
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
           obj.SetActive(false);
           pool.InactiveObjects.Add(obj);
       }
   }
   
   static GameObject SetParentObject(PoolType poolType)
   {
       switch (poolType)
       {
           case PoolType.ImpactHits:
               return _impactHitsEmpty;
           case PoolType.GameObject:
               return _gameObjectEmpty;
           case PoolType.None:
               return null;
           case PoolType.Enemy:
               return _enemyEmpty;
           case PoolType.Projectiles:
               return _projectilesEmpty;
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
    public int PoolSize = 100;
}

using System;
using UnityEngine;
using UnityEngine.Pool;

public class PooledGameObject
{
    public GameObject Instance { get; set; }
    public Transform ParentTransform { get; set; }
    // Additional data can be added here
}
public class CustomObjectPool<T> where T : class
{
    ObjectPool<T> pool;
    Func<GameObject, Vector3, Transform, T> createFunc;
    

    public CustomObjectPool(
        Func<GameObject, Vector3, Transform, T> customCreateFunc, 
        Action<T> onGet = null,
        Action<T> onRelease = null, 
        Action<T> onDestroy = null, 
        bool collectionCheck = true, 
        int defaultCapacity = 10,
        int maxSize = 10000)
    {
        this.createFunc = customCreateFunc;
        this.pool = new ObjectPool<T>(
            () => this.createFunc(null, default(Vector3), null), // Default values or manage via methods
            onGet,
            onRelease,
            onDestroy,
            collectionCheck,
            defaultCapacity,
            maxSize);
    }

    public T Get(GameObject prefab, Vector3 position, Transform parent = null)
    {
        // if (pool.CountInactive > 0)
        // {
        //     T item = pool.Get();
        //     return item;
        // }
        // return createFunc(prefab, position); // Only create a new object if none are available
        
        T item;
        if (pool.CountInactive > 0)
        {
            item = pool.Get(); // This will call the actionOnGet if set
        }
        else
        {
            item = createFunc(prefab, position, parent); // Create a new object if none are available
           
        }
        return item;
    }

    // Expose other methods as needed
    public void Release(T item)
    {
        pool.Release(item);
    }

    public void Clear()
    {
        pool.Clear();
    }
}

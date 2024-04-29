using System;
using UnityEngine;
using UnityEngine.Pool;


public class CustomObjectPool<T> where T : class
{
    ObjectPool<T> pool;
    readonly Func<GameObject, Vector3, ObjectPoolManager.PoolType, T> _createFunc;

    public CustomObjectPool(
        Func<GameObject, Vector3, ObjectPoolManager.PoolType, T> customCreateFunc, 
        Action<T> onGet = null, 
        Action<T> onRelease = null, 
        Action<T> onDestroy = null, 
        bool collectionCheck = true, 
        int defaultCapacity = 10,
        int maxSize = 10000)
    {
        this._createFunc = customCreateFunc;
        this.pool = new ObjectPool<T>(
            () => this._createFunc(null, default(Vector3), default(ObjectPoolManager.PoolType)), // Default values or manage via methods
            onGet,
            onRelease,
            onDestroy,
            collectionCheck,
            defaultCapacity,
            maxSize);
    }

    public T Get(GameObject prefab, Vector3 position, ObjectPoolManager.PoolType enumType = default(ObjectPoolManager.PoolType))
    {
        T item;
        if (pool.CountInactive > 0)
        {
            item = pool.Get(); // This will call the actionOnGet if set
        }
        else
        {
            item = _createFunc(prefab, position, enumType); // Create a new object if none are available
           
        }
        return item;
    }

    public void Release(T item)
    {
        pool.Release(item);
    }

    public void Clear()
    {
        pool.Clear();
    }
}

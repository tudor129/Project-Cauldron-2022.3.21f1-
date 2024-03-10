using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEnemyCulling : MonoBehaviour
{
    public float _cullDistance = 100f;
    

    [SerializeField] Camera _camera;
    Plane[] _planes;

    void Awake()
    {
        _planes = GeometryUtility.CalculateFrustumPlanes(_camera);
    }

    void Update()
    {
        _planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        foreach (Enemy enemy in Enemy.ActiveEnemies)
        {
            SkinnedMeshRenderer renderer = enemy.GetComponentInChildren<SkinnedMeshRenderer>();
            if (renderer)
            {
                if (renderer)
                {
                    bool isWithinCullDistance = Vector3.Distance(_camera.transform.position, enemy.transform.position) <= _cullDistance;
                    bool isVisible = isWithinCullDistance && GeometryUtility.TestPlanesAABB(_planes, renderer.bounds);
                
                    renderer.enabled = isVisible;
                }
            }
        }
    }
}

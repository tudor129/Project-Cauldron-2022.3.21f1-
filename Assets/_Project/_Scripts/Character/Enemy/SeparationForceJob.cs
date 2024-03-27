using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct SeparationForceJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> enemyPositions;
    public float3 myPosition;
    public float desiredSeparation;
    public float moveSpeed;
    public NativeArray<float3> results;

    public void Execute(int index)
    {
        float3 positionDifference = myPosition - enemyPositions[index];
        float d = math.length(positionDifference);
        
        float3 force = float3.zero;
        if (d > 0 && d < desiredSeparation)
        {
            positionDifference = math.normalize(positionDifference);
            positionDifference /= d;
            force += positionDifference;
        }
      

        results[index] = force;
    }
}

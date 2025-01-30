using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthBarSystem : ISystem
{
    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float3 cameraForfard = Vector3.zero;
        if (Camera.main != null)
        {
            cameraForfard = Camera.main.transform.forward;
        }
        
        foreach ((
            RefRW<LocalTransform> localTransform, 
            RefRO<HealthBar> healthBar) 
            in SystemAPI.Query<
                RefRW<LocalTransform>, 
                RefRO<HealthBar>>())
        {      
            LocalTransform parentLocalTransform = SystemAPI.GetComponent<LocalTransform>(healthBar.ValueRO.healthEntity);
            
            if (localTransform.ValueRW.Scale == 1f)
            {
                localTransform.ValueRW.Rotation = 
                    parentLocalTransform.InverseTransformRotation(quaternion.LookRotation(cameraForfard, math.up()));
            }
            
            Health health = SystemAPI.GetComponent<Health>(healthBar.ValueRO.healthEntity);

            if (!health.onHealthChange)
                continue;

            float healthNormilize = (float)health.healthAmount / health.healthAmountMax;

            localTransform.ValueRW.Scale = healthNormilize == 1f ? 0f : 1f;

            RefRW<PostTransformMatrix> barVisualPostTransformMatrix = SystemAPI.GetComponentRW<PostTransformMatrix>(healthBar.ValueRO.barVisualEntity);
            barVisualPostTransformMatrix.ValueRW.Value = float4x4.Scale(healthNormilize, 1, 1);
        }
    }
}


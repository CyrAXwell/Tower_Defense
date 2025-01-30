using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct HeatAreaSkillDestroySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<HeatArea> heatArea,
            Entity entity) 
            in SystemAPI.Query<
                RefRW<HeatArea>>().WithEntityAccess())
        {
            heatArea.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (heatArea.ValueRO.timer > 0f)
                continue;

            entityCommandBuffer.DestroyEntity(entity);
            
        }
    }
}

using Unity.Burst;
using Unity.Entities;

partial struct HealingAreaSkillDestroySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<HealingArea> healingArea,
            Entity entity) 
            in SystemAPI.Query<
                RefRW<HealingArea>>().WithEntityAccess())
        {  
            healingArea.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (healingArea.ValueRO.timer > 0)
                continue;

            entityCommandBuffer.DestroyEntity(entity);
        }
    }
}

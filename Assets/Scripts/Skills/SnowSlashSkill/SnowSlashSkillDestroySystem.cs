using Unity.Burst;
using Unity.Entities;

partial struct SnowSlashSkillDestroySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<SnowSlash> snowSlash,
            Entity entity) 
            in SystemAPI.Query<
                RefRW<SnowSlash>>().WithEntityAccess())
        {
            snowSlash.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (snowSlash.ValueRO.timer > 0f)
                continue;

            entityCommandBuffer.DestroyEntity(entity);
            
        }
    }
}

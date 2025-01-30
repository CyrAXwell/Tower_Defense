using Unity.Burst;
using Unity.Entities;

partial struct PoisonAreaSkillDestroySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<PoisonArea> poisonArea,
            Entity entity) 
            in SystemAPI.Query<
                RefRW<PoisonArea>>().WithEntityAccess())
        {
            poisonArea.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (poisonArea.ValueRO.timer > 0f)
                continue;

            entityCommandBuffer.DestroyEntity(entity);
        }
    }
}

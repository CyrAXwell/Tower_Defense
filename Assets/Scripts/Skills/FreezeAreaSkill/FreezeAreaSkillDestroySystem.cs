using Unity.Burst;
using Unity.Entities;

partial struct FreezeAreaSkillDestroySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<FreezeArea> frezeArea,
            Entity entity) 
            in SystemAPI.Query<
                RefRW<FreezeArea>>().WithEntityAccess())
        {
            frezeArea.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (frezeArea.ValueRO.timer > 0f)
                continue;

            entityCommandBuffer.DestroyEntity(entity);            
        }
    }
}

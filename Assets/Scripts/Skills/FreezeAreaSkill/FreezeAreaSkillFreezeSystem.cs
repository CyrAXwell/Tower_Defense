using Unity.Burst;
using Unity.Entities;

partial struct FreezeAreaSkillFreezeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<UnitMover> unitMover, 
            RefRW<Freezed> freezed,
            Entity entity) 
            in SystemAPI.Query<
                RefRW<UnitMover>,
                RefRW<Freezed>>().WithEntityAccess())
        {   
            freezed.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (freezed.ValueRO.timer > 0)
                continue;
            
            unitMover.ValueRW.moveSpeed = freezed.ValueRO.notFreezedMoveSpeed;
            unitMover.ValueRW.rotationSpeed = freezed.ValueRO.notFreezedRotationSpeed;

            entityCommandBuffer.SetComponentEnabled<Freezed>(entity, false);
        }
    }
}

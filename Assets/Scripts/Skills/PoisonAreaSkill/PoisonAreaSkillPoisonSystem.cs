using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct PoisonAreaSkillPoisonSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<Poisoned> poisoned,
            Entity entity) 
            in SystemAPI.Query<
                RefRW<Poisoned>>().WithEntityAccess())
        {   
            poisoned.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (poisoned.ValueRO.timer <= 0)
            {
                entityCommandBuffer.SetComponentEnabled<Poisoned>(entity, false);
                continue;
            }

            poisoned.ValueRW.damageTimer -= SystemAPI.Time.DeltaTime;
            if (poisoned.ValueRO.damageTimer > 0)
                continue;
            
            poisoned.ValueRW.damageTimer = poisoned.ValueRO.damageFrequency;

            RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(entity);
            targetHealth.ValueRW.healthAmount -= poisoned.ValueRO.damageAmount;
            targetHealth.ValueRW.onHealthChange = true;
        }
    }
}

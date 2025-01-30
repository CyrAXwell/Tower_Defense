using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthDeadTestSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach((
            RefRO<Health> health, 
            RefRO<Unit> unit,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<Health>,
                RefRO<Unit>>().WithEntityAccess())
        {
            if (health.ValueRO.healthAmount <= 0)
            {
                if (unit.ValueRO.faction == Faction.Enemy)
                {
                    RefRW<Enemy> enemy = SystemAPI.GetComponentRW<Enemy>(entity);
                    RefRW<PlayerLevel> player = SystemAPI.GetSingletonRW<PlayerLevel>();
                    player.ValueRW.experiencePoints += enemy.ValueRO.experiencePointsDropAmount;
                    player.ValueRW.onExperiencePointsChange = true;

                    Entity enemyAmountCounterEntity = SystemAPI.GetSingletonEntity<EnemyAmountCounter>();
                    RefRW<EnemyAmountCounter> enemyAmountCounter = SystemAPI.GetComponentRW<EnemyAmountCounter>(enemyAmountCounterEntity);
                    enemyAmountCounter.ValueRW.destroyedEnemyAmount++;
                }

                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}

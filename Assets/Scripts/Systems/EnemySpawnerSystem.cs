using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemySpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<EntitiesReferences>(out EntitiesReferences entitiesReferences))
        {
            return;
        }

        foreach((
            RefRO<LocalTransform> localTransform, 
            RefRW<EnemySpawner> enemySpawner) 
            in SystemAPI.Query<
                RefRO<LocalTransform>, 
                RefRW<EnemySpawner>>())
        {
            enemySpawner.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (enemySpawner.ValueRO.timer > 0f)
            {
                continue;
            }
            enemySpawner.ValueRW.timer = enemySpawner.ValueRW.timerMax;
            
            Entity lightMeleeUnitEntity = state.EntityManager.Instantiate(entitiesReferences.lightMeleeUnitPrefabEntity);
            Unity.Mathematics.Random random = enemySpawner.ValueRO.random;
            float radius = enemySpawner.ValueRO.radius;
            float3 spawnOffset = new float3(random.NextFloat(-radius, radius), 0, random.NextFloat(-radius, radius));

            LocalTransform lightMeleeUnitLocalTransform = state.EntityManager.GetComponentData<LocalTransform>(lightMeleeUnitEntity);
            lightMeleeUnitLocalTransform.Position = localTransform.ValueRO.Position + spawnOffset;
            lightMeleeUnitLocalTransform.Rotation = quaternion.Euler(new float3(0, 180, 0));
            state.EntityManager.SetComponentData(lightMeleeUnitEntity, lightMeleeUnitLocalTransform);

            enemySpawner.ValueRW.random = random;

            Entity enemyAmountCounterEntity = SystemAPI.GetSingletonEntity<EnemyAmountCounter>();
            RefRW<EnemyAmountCounter> enemyAmountCounter = SystemAPI.GetComponentRW<EnemyAmountCounter>(enemyAmountCounterEntity);
            enemyAmountCounter.ValueRW.spawnedEnemyAmount++;
        } 
    }
}

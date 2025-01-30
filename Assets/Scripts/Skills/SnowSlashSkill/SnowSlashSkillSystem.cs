using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct SnowSlashSkillSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);
        
        foreach ((
            RefRO<LocalTransform> localTransform, 
            RefRW<SnowSlash> snowSlash,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRW<SnowSlash>>().WithEntityAccess())
        {            
            snowSlash.ValueRW.damageTimer -= SystemAPI.Time.DeltaTime;
            if (snowSlash.ValueRO.damageTimer > 0)
                continue;

            snowSlash.ValueRW.damageTimer = snowSlash.ValueRO.damageFrequency;
            
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };

            distanceHitList.Clear();
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, snowSlash.ValueRO.size / 2, ref distanceHitList, collisionFilter))
            {
                foreach( DistanceHit distanceHit in distanceHitList)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                        continue;

                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (snowSlash.ValueRO.enemyTarget == targetUnit.faction)
                    {
                        RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(distanceHit.Entity);
                        targetHealth.ValueRW.healthAmount -= snowSlash.ValueRO.damageAmount;
                        targetHealth.ValueRW.onHealthChange = true;
                    }
                }
            }
        }
    }
}

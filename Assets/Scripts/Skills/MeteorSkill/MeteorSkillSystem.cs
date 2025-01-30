using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct MeteorSkillSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);
        
        foreach ((
            RefRO<LocalTransform> localTransform, 
            RefRW<Meteor> meteor,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRW<Meteor>>().WithEntityAccess())
        {            
            meteor.ValueRW.damageDelayTimer -= SystemAPI.Time.DeltaTime;
            if (meteor.ValueRO.damageDelayTimer > 0)
                continue;

            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };

            distanceHitList.Clear();
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, meteor.ValueRO.size / 2, ref distanceHitList, collisionFilter))
            {
                foreach( DistanceHit distanceHit in distanceHitList)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                        continue;

                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (meteor.ValueRO.enemyTarget == targetUnit.faction)
                    {
                        RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(distanceHit.Entity);
                        targetHealth.ValueRW.healthAmount -= meteor.ValueRO.damageAmount;
                        targetHealth.ValueRW.onHealthChange = true;
                    }
                }
            }

            entityCommandBuffer.DestroyEntity(entity);
        }
    }
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct PoisonAreaSkillSystem : ISystem
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
            RefRW<PoisonArea> poisonArea,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRW<PoisonArea>>().WithEntityAccess())
        {            
            poisonArea.ValueRW.poisonTimer -= SystemAPI.Time.DeltaTime;
            if (poisonArea.ValueRO.poisonTimer > 0)
                continue;

            poisonArea.ValueRW.poisonTimer = poisonArea.ValueRO.poisonFrequency;
            
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };

            distanceHitList.Clear();
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, poisonArea.ValueRO.size / 2, ref distanceHitList, collisionFilter))
            {
                foreach( DistanceHit distanceHit in distanceHitList)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                        continue;

                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (poisonArea.ValueRO.enemyTarget == targetUnit.faction)
                    {
                        bool isEntityAlreadyPoisoned = true;
                        if (!SystemAPI.HasComponent<Poisoned>(distanceHit.Entity))
                        {
                            entityCommandBuffer.AddComponent<Poisoned>(distanceHit.Entity);
                            isEntityAlreadyPoisoned = false;
                        }
                        else if (!SystemAPI.IsComponentEnabled<Poisoned>(distanceHit.Entity))
                        {
                            isEntityAlreadyPoisoned = false;
                        }
                        else
                        {
                            RefRW<Poisoned> poisoned = SystemAPI.GetComponentRW<Poisoned>(distanceHit.Entity);
                            poisoned.ValueRW.timer = poisonArea.ValueRO.poisonDuration;
                        }

                        if (!isEntityAlreadyPoisoned)
                        {
                            entityCommandBuffer.SetComponentEnabled<Poisoned>(distanceHit.Entity, true);
                            entityCommandBuffer.SetComponent(distanceHit.Entity, new Poisoned { 
                                timer = poisonArea.ValueRO.poisonDuration,
                                duration = poisonArea.ValueRO.poisonDuration,
                                damageAmount = poisonArea.ValueRO.damageAmount,
                                damageFrequency = poisonArea.ValueRO.damageFrequency,
                            });
                        }
                        
                    }
                }
            }
        }
    }
}
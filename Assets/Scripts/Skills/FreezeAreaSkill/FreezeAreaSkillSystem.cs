using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct FreezeAreaSkillSystem : ISystem
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
            RefRW<FreezeArea> freezeArea,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRW<FreezeArea>>().WithEntityAccess())
        {            
            freezeArea.ValueRW.damageTimer -= SystemAPI.Time.DeltaTime;
            if (freezeArea.ValueRO.damageTimer > 0)
                continue;

            freezeArea.ValueRW.damageTimer = freezeArea.ValueRO.damageFrequency;
            
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };

            distanceHitList.Clear();
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, freezeArea.ValueRO.size / 2, ref distanceHitList, collisionFilter))
            {
                foreach( DistanceHit distanceHit in distanceHitList)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                        continue;

                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (freezeArea.ValueRO.enemyTarget == targetUnit.faction)
                    {
                        RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(distanceHit.Entity);
                        targetHealth.ValueRW.healthAmount -= freezeArea.ValueRO.damageAmount;
                        targetHealth.ValueRW.onHealthChange = true;

                        if (!SystemAPI.HasComponent<UnitMover>(distanceHit.Entity))
                            continue;

                        bool isEntityAlreadyFreezed = true;
                        if (!SystemAPI.HasComponent<Freezed>(distanceHit.Entity))
                        {
                            entityCommandBuffer.AddComponent<Freezed>(distanceHit.Entity);
                            isEntityAlreadyFreezed = false;
                        }
                        else if (!SystemAPI.IsComponentEnabled<Freezed>(distanceHit.Entity))
                        {
                            isEntityAlreadyFreezed = false;
                        }
                        else
                        {
                            RefRW<Freezed> freezed = SystemAPI.GetComponentRW<Freezed>(distanceHit.Entity);
                            freezed.ValueRW.timer = freezeArea.ValueRO.freezeDuration;
                        }

                        if (!isEntityAlreadyFreezed)
                        {
                            entityCommandBuffer.SetComponentEnabled<Freezed>(distanceHit.Entity, true);

                            RefRW<UnitMover> unitMover = SystemAPI.GetComponentRW<UnitMover>(distanceHit.Entity);
                            entityCommandBuffer.SetComponent(distanceHit.Entity, new Freezed { 
                                timer = freezeArea.ValueRO.freezeDuration,
                                notFreezedMoveSpeed = unitMover.ValueRO.moveSpeed,
                                notFreezedRotationSpeed = unitMover.ValueRO.rotationSpeed
                            });

                            unitMover.ValueRW.moveSpeed = unitMover.ValueRO.moveSpeed * freezeArea.ValueRO.freezeMultiplayer;
                            unitMover.ValueRW.rotationSpeed = unitMover.ValueRO.rotationSpeed * freezeArea.ValueRO.freezeMultiplayer;
                        }
                    }
                }
            }
        }
    }
}

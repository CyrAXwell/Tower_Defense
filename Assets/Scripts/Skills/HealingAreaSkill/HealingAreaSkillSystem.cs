using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct HealingAreaSkillSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);

        foreach ((
            RefRO<LocalTransform> localTransform, 
            RefRW<HealingArea> healingArea,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRW<HealingArea>>().WithEntityAccess())
        {          
            healingArea.ValueRW.healTimer -= SystemAPI.Time.DeltaTime;
            if (healingArea.ValueRO.healTimer > 0)
                continue;

            healingArea.ValueRW.healTimer = healingArea.ValueRO.healFrequency;

            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };

            distanceHitList.Clear();
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, healingArea.ValueRO.size / 2, ref distanceHitList, collisionFilter))
            {
                foreach (DistanceHit distanceHit in distanceHitList)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                        continue;

                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (healingArea.ValueRO.friendlyTarget == targetUnit.faction)
                    {
                        RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(distanceHit.Entity);
                        if (targetHealth.ValueRO.healthAmount == targetHealth.ValueRO.healthAmountMax)
                            continue;
                            
                        if (targetHealth.ValueRO.healthAmount + healingArea.ValueRO.healAmount <= targetHealth.ValueRO.healthAmountMax)
                        {
                            targetHealth.ValueRW.healthAmount += healingArea.ValueRO.healAmount;
                        }
                        else
                        {
                            targetHealth.ValueRW.healthAmount = targetHealth.ValueRO.healthAmountMax;
                        }

                        targetHealth.ValueRW.onHealthChange = true;
                    }
                }
            }
        }
    }
}

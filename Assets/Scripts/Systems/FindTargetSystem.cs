using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct FindTargetSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);

        foreach((
            RefRO<LocalTransform> localTransform, 
            RefRW<FindTarget> findTarget, 
            RefRW<Target> target) 
            in SystemAPI.Query<
                RefRO<LocalTransform>, 
                RefRW<FindTarget>, 
                RefRW<Target>>())
        {
            findTarget.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (findTarget.ValueRO.timer > 0f)
                continue;

            findTarget.ValueRW.timer = findTarget.ValueRW.timerMax;

            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };

            distanceHitList.Clear();
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, findTarget.ValueRO.range, ref distanceHitList, collisionFilter))
            {
                DistanceHit closestTarget = new DistanceHit();
                foreach( DistanceHit distanceHit in distanceHitList)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                        continue;

                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (targetUnit.faction == findTarget.ValueRO.targetFaction)
                    {
                        if (closestTarget.Entity == Entity.Null)
                        {
                            closestTarget = distanceHit;
                            target.ValueRW.targetEntity = distanceHit.Entity;
                        }
                        else if (closestTarget.Distance > distanceHit.Distance)
                        {
                            closestTarget = distanceHit;
                            target.ValueRW.targetEntity = distanceHit.Entity;
                        }
                    }
                }
                
            }
        }
    }
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct HeatAreaSkillSystem : ISystem
{
    //private const float SIZE_MULTIPLIER = 0.25f;
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);
        
        // foreach ((
        //     RefRO<LocalTransform> localTransform, 
        //     RefRW<HeatArea> heatArea,
        //     HeatAreaVisualGameObject visualGameObject, 
        //     Entity entity) 
        //     in SystemAPI.Query<
        //         RefRO<LocalTransform>,
        //         RefRW<HeatArea>, 
        //         HeatAreaVisualGameObject>().WithDisabled<SkillVisualized>().WithEntityAccess())
        // {
        //     GameObject newHeatAreaVisualiseGameObject = GameObject.Instantiate(visualGameObject.gameObjectPrefab);
        //     newHeatAreaVisualiseGameObject.transform.position = localTransform.ValueRO.Position;
        //     newHeatAreaVisualiseGameObject.transform.rotation = localTransform.ValueRO.Rotation;
        //     float localScale = heatArea.ValueRO.size * SIZE_MULTIPLIER;
        //     newHeatAreaVisualiseGameObject.transform.localScale = new Vector3(localScale, 1, localScale);

        //     visualGameObject.gameObject = newHeatAreaVisualiseGameObject;
        //     heatArea.ValueRW.timer = heatArea.ValueRO.duration;
        //     SystemAPI.SetComponentEnabled<SkillVisualized>(entity, true);
        // }

        foreach ((
            RefRO<LocalTransform> localTransform, 
            RefRW<HeatArea> heatArea,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRW<HeatArea>>().WithEntityAccess())
        {
            //Debug.Log("Skill System");
            
            heatArea.ValueRW.damageTimer -= SystemAPI.Time.DeltaTime;
            if (heatArea.ValueRO.damageTimer > 0)
                continue;

            heatArea.ValueRW.damageTimer = heatArea.ValueRO.damageFrequency;
            
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };

            distanceHitList.Clear();
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, heatArea.ValueRO.size / 2, ref distanceHitList, collisionFilter))
            {
                foreach( DistanceHit distanceHit in distanceHitList)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                        continue;

                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (heatArea.ValueRO.enemyTarget == targetUnit.faction)
                    {
                        RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(distanceHit.Entity);
                        targetHealth.ValueRW.healthAmount -= heatArea.ValueRO.damageAmount;
                        targetHealth.ValueRW.onHealthChange = true;
                    }
                }
            }
        }

        // foreach ((
        //     RefRW<HeatArea> heatArea,
        //     HeatAreaVisualGameObject visualGameObject,
        //     Entity entity) 
        //     in SystemAPI.Query<
        //         RefRW<HeatArea>,
        //         HeatAreaVisualGameObject>().WithAll<SkillVisualized>().WithEntityAccess())
        // {
        //     heatArea.ValueRW.timer -= SystemAPI.Time.DeltaTime;
        //     if (heatArea.ValueRO.timer > 0f)
        //         continue;

        //     GameObject.Destroy(visualGameObject.gameObject);

        //     entityCommandBuffer.DestroyEntity(entity);
            
        // }
    }
}

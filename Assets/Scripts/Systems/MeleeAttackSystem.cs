using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
//using UnityEngine;

partial struct MeleeAttackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<RaycastHit> raycastHitList = new NativeList<RaycastHit>(Allocator.Temp);
        
        foreach ((
            RefRO<LocalTransform> localTransform, 
            RefRW<MeleeAttack> meleeAttack,
            RefRO<Target> target,
            RefRW<UnitMover> unitMover) 
            in SystemAPI.Query<
                RefRO<LocalTransform>, 
                RefRW<MeleeAttack>,
                RefRO<Target>,
                RefRW<UnitMover>>())
        {
            if (target.ValueRO.targetEntity == Entity.Null)
            {
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            float meleeAttackDistanceSq = 2f;
            bool isCloseEnoughToAttack = math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position) < meleeAttackDistanceSq;
            
            bool isTouchingTarget = false;
            if (!isCloseEnoughToAttack)
            {
                float3 directionTotarget = targetLocalTransform.Position - localTransform.ValueRO.Position;
                directionTotarget = math.normalize(directionTotarget);
                float distanceExtraToTestRaycast = 0.5f;
                RaycastInput raycastInput = new RaycastInput { 
                    Start = localTransform.ValueRO.Position, 
                    End = localTransform.ValueRO.Position + directionTotarget * (meleeAttack.ValueRO.colliderSize + distanceExtraToTestRaycast), 
                    Filter = CollisionFilter.Default
                };
                raycastHitList.Clear();
                if (collisionWorld.CastRay(raycastInput, ref raycastHitList))
                {
                    foreach (RaycastHit raycastHit in raycastHitList)
                    {
                        if (raycastHit.Entity == target.ValueRO.targetEntity)
                        {
                            isTouchingTarget = true;
                            break; 
                        }
                    }
                }
            }

            if (!isCloseEnoughToAttack && !isTouchingTarget)
            {
                unitMover.ValueRW.targetPosition = targetLocalTransform.Position;
            }
            else
            {
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;
                meleeAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;

                if (meleeAttack.ValueRW.timer > 0)
                    continue;
                
                meleeAttack.ValueRW.timer = meleeAttack.ValueRW.timerMax;

                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.healthAmount -= meleeAttack.ValueRO.damageAmount;
                targetHealth.ValueRW.onHealthChange = true;

                meleeAttack.ValueRW.onAttack = true;
            }

        } 
    }
}

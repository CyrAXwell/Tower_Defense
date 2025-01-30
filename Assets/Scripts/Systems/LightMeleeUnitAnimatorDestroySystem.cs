using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(LightMeleeUnitAnimatorSystem))]
partial struct LightMeleeUnitAnimatorDestroySystem : ISystem
{
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach ((
            LightMeleeUnitAnimator botAnimator, 
            Entity entity) 
            in SystemAPI.Query<
                LightMeleeUnitAnimator>().WithEntityAccess().WithNone<LightMeleeUnitVisualGameObjectPrefab, LocalTransform>())
        {
            if (botAnimator.animator == null)
                continue;

            GameObject.Destroy(botAnimator.animator.gameObject);
            entityCommandBuffer.RemoveComponent<LightMeleeUnitAnimator>(entity);    
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}

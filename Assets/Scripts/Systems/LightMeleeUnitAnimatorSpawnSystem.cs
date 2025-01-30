using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct LightMeleeUnitAnimatorSpawnSystem : ISystem
{
    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach ((
            LightMeleeUnitVisualGameObjectPrefab botGameObjectPrefab, 
            LocalTransform localTransform,
            Entity entity) 
            in SystemAPI.Query<
                LightMeleeUnitVisualGameObjectPrefab,
                LocalTransform>().WithEntityAccess().WithNone<LightMeleeUnitAnimator>() )
        {
            GameObject newCompanionGameObject = GameObject.Instantiate(botGameObjectPrefab.gameObjectPrefab);
            newCompanionGameObject.transform.position = localTransform.Position;
            newCompanionGameObject.transform.rotation = localTransform.Rotation;

            LightMeleeUnitAnimator newBotAnimator = new LightMeleeUnitAnimator {
                animator = newCompanionGameObject.GetComponent<Animator>()
            };

            entityCommandBuffer.AddComponent(entity, newBotAnimator);
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}

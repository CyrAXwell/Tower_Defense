using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PoisonAreaSkillSpawnSystem : ISystem
{
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // foreach ((
        //     RefRO<LocalTransform> localTransform, 
        //     RefRW<PoisonArea> poisonArea,
        //     PoisonAreaVisualGameObject visualGameObject, 
        //     Entity entity) 
        //     in SystemAPI.Query<
        //         RefRO<LocalTransform>,
        //         RefRW<PoisonArea>, 
        //         PoisonAreaVisualGameObject>().WithDisabled<SkillVisualized>().WithEntityAccess())
        // {
        //     GameObject newPoisonAreaVisualiseGameObject = GameObject.Instantiate(visualGameObject.gameObjectPrefab);
        //     newPoisonAreaVisualiseGameObject.transform.position = localTransform.ValueRO.Position + new float3(0, 0.01f, 0);
        //     newPoisonAreaVisualiseGameObject.transform.rotation = localTransform.ValueRO.Rotation;

        //     PoisonAreaSkillVisualController visualController = newPoisonAreaVisualiseGameObject.GetComponent<PoisonAreaSkillVisualController>();
        //     visualController.SetParameters(poisonArea.ValueRO.size, poisonArea.ValueRO.duration);

        //     visualGameObject.gameObject = newPoisonAreaVisualiseGameObject;
        //     poisonArea.ValueRW.timer = poisonArea.ValueRO.duration;
        //     SystemAPI.SetComponentEnabled<SkillVisualized>(entity, true);
        // } 
    }
}

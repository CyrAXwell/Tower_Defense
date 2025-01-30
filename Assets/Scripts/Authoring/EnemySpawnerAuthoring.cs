using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public float timerMax; 
    public float radius;

    public class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemySpawner{
                timerMax = authoring.timerMax,
                radius = authoring.radius,
                random = new Unity.Mathematics.Random((uint)entity.Index),
            });
        }
    }
}

public struct EnemySpawner : IComponentData
{
    public float timer;
    public float timerMax; 
    public float radius;
    public Unity.Mathematics.Random random;
}

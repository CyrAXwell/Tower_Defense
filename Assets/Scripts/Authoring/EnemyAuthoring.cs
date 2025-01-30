using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public int experiencePointsDropAmount;

    public class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Enemy{
                experiencePointsDropAmount = authoring.experiencePointsDropAmount,
            }); 
        }
    }
}

public struct Enemy : IComponentData
{
    public int experiencePointsDropAmount;
}

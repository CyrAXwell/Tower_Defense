using Unity.Entities;
using UnityEngine;

public class EnemyAmountCounterAuthoring : MonoBehaviour
{
    public class Baker : Baker<EnemyAmountCounterAuthoring>
    {
        public override void Bake(EnemyAmountCounterAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyAmountCounter{} );
        }
    }
}

public struct EnemyAmountCounter : IComponentData
{
    public int spawnedEnemyAmount;
    public int destroyedEnemyAmount;
}

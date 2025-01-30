using Unity.Entities;
using UnityEngine;

public class SnowSlashAuthoring : MonoBehaviour
{
    public float duration;
    public int damageAmount;
    public float damageFrequency;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;

    public class Baker : Baker<SnowSlashAuthoring>
    {
        public override void Bake(SnowSlashAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SnowSlash {
                duration = authoring.duration,
                damageAmount = authoring.damageAmount,
                damageFrequency = authoring.damageFrequency,
                size = authoring.size,
                enemyTarget = authoring.enemyTarget,
                upgradeDamageAmount = authoring.upgradeDamageAmount,
            });
        }
    }
}

public struct SnowSlash : IComponentData
{
    public float timer;
    public float duration;
    public int damageAmount;
    public float damageFrequency;
    public float damageTimer;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
}
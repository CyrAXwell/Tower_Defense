using Unity.Entities;
using UnityEngine;

public class HeatAreaAuthoring : MonoBehaviour
{
    public float duration;
    public int damageAmount;
    public float damageFrequency;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;

    public class Baker : Baker<HeatAreaAuthoring>
    {
        public override void Bake(HeatAreaAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HeatArea {
                duration = authoring.duration,
                damageAmount = authoring.damageAmount,
                damageFrequency = authoring.damageFrequency,
                size = authoring.size,
                enemyTarget = authoring.enemyTarget,
                upgradeDamageAmount = authoring.upgradeDamageAmount,
                upgradeSize = authoring.upgradeSize,
            });
        }
    }
}

public struct HeatArea : IComponentData
{
    public float timer;
    public float duration;
    public int damageAmount;
    public float damageTimer;
    public float damageFrequency;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;
}
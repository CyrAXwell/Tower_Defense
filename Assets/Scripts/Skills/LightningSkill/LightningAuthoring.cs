using Unity.Entities;
using UnityEngine;

public class LightningAuthoring : MonoBehaviour
{
    public float duration;
    public float damageDelay;
    public int damageAmount;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;

    public class Baker : Baker<LightningAuthoring>
    {
        public override void Bake(LightningAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Lightning {
                duration = authoring.duration,
                damageDelay = authoring.damageDelay,
                damageAmount = authoring.damageAmount,
                size = authoring.size,
                enemyTarget = authoring.enemyTarget,
                upgradeDamageAmount = authoring.upgradeDamageAmount,
                upgradeSize = authoring.upgradeSize,
            });
        }
    }
}

public struct Lightning : IComponentData
{
    public float duration;
    public float damageDelay;
    public float damageDelayTimer;
    public int damageAmount;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;
}

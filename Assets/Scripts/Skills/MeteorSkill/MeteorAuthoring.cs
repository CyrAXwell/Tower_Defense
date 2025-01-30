using Unity.Entities;
using UnityEngine;

public class MeteorAuthoring : MonoBehaviour
{
    public float duration;
    public float damageDelay;
    public int damageAmount;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;

    public class Baker : Baker<MeteorAuthoring>
    {
        public override void Bake(MeteorAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Meteor {
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

public struct Meteor : IComponentData
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

using Unity.Entities;
using UnityEngine;

public class FreezeAreaAuthoring : MonoBehaviour
{
    public float duration;
    public int damageAmount;
    public float damageFrequency;
    public float freezeDuration;
    public float freezeMultiplayer;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;
    public float upgradefreezeMultiplayer;

    public class Baker : Baker<FreezeAreaAuthoring>
    {
        public override void Bake(FreezeAreaAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FreezeArea {
                duration = authoring.duration,
                damageAmount = authoring.damageAmount,
                damageFrequency = authoring.damageFrequency,
                freezeDuration = authoring.freezeDuration,
                freezeMultiplayer = authoring.freezeMultiplayer,
                size = authoring.size,
                enemyTarget = authoring.enemyTarget,
                upgradeDamageAmount = authoring.upgradeDamageAmount,
                upgradeSize = authoring.upgradeSize,
                upgradefreezeMultiplayer = authoring.upgradefreezeMultiplayer,
            });
        }
    }
}

public struct FreezeArea : IComponentData
{
    public float timer;
    public float duration;
    public int damageAmount;
    public float damageTimer;
    public float damageFrequency;
    public float freezeDuration;
    public float freezeMultiplayer;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;
    public float upgradefreezeMultiplayer;
}

public struct Freezed : IComponentData, IEnableableComponent
{
    public float timer;
    public float notFreezedMoveSpeed;
    public float notFreezedRotationSpeed;
}

using Unity.Entities;
using UnityEngine;

public class HealingAreaAuthoring : MonoBehaviour
{
    public float duration;
    public int healAmount;
    public float healFrequency;
    public float size;
    public Faction friendlyTarget;
    public int upgradeHealAmount;

    public class Baker : Baker<HealingAreaAuthoring>
    {
        public override void Bake(HealingAreaAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HealingArea {
                duration = authoring.duration,
                healAmount = authoring.healAmount,
                healFrequency = authoring.healFrequency,
                size = authoring.size,
                friendlyTarget = authoring.friendlyTarget,
                upgradeHealAmount = authoring.upgradeHealAmount,
            });
        }
    }
}

public struct HealingArea : IComponentData
{
    public float timer;
    public float duration;
    public int healAmount;
    public float healFrequency;
    public float healTimer;
    public float size;
    public Faction friendlyTarget;
    public int upgradeHealAmount;
}
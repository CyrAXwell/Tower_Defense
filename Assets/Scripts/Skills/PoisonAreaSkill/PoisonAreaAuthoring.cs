using Unity.Entities;
using UnityEngine;

public class PoisonAreaAuthoring : MonoBehaviour
{
    public GameObject gameObjectPrefab;
    public float duration;
    public int damageAmount;
    public float damageFrequency;
    public float poisonDuration;
    public float poisonFrequency;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;

    public class Baker : Baker<PoisonAreaAuthoring>
    {
        public override void Bake(PoisonAreaAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PoisonArea {
                duration = authoring.duration,
                damageAmount = authoring.damageAmount,
                damageFrequency = authoring.damageFrequency,
                poisonDuration = authoring.poisonDuration,
                poisonFrequency = authoring.poisonFrequency,
                size = authoring.size,
                enemyTarget = authoring.enemyTarget,
                upgradeDamageAmount = authoring.upgradeDamageAmount,
                upgradeSize = authoring.upgradeSize,
            });
        }
    }
}

public struct PoisonArea : IComponentData
{
    public float timer;
    public float duration;
    public int damageAmount;
    public float damageFrequency;
    public float poisonDuration;
    public float poisonTimer;
    public float poisonFrequency;
    public float size;
    public Faction enemyTarget;
    public int upgradeDamageAmount;
    public float upgradeSize;

}

public struct Poisoned : IComponentData, IEnableableComponent
{
    public float timer;
    public float duration;
    public float damageTimer;
    public int damageAmount;
    public float damageFrequency;
}

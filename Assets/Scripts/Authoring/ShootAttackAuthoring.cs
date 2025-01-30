using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ShootAttackAuthoring : MonoBehaviour
{
    public float timerMax;
    public int damageAmount;
    public float bulletSpeed;
    public float attackDistance;
    public GameObject gunChassisGameObject;
    public GameObject bulletSpawnPointGameObject;

    public class Baker : Baker<ShootAttackAuthoring>
    {
        public override void Bake(ShootAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShootAttack { 
                timerMax = authoring.timerMax,
                damageAmount = authoring.damageAmount,
                bulletSpeed = authoring.bulletSpeed,
                attackDistance = authoring.attackDistance,
                gunChassisEntity = GetEntity(authoring.gunChassisGameObject, TransformUsageFlags.Dynamic),
                bulletSpawnPointEntity = GetEntity(authoring.bulletSpawnPointGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct ShootAttack : IComponentData
{
    public float timer;
    public float timerMax;
    public int damageAmount;
    public float bulletSpeed;
    public float attackDistance;
    public Entity gunChassisEntity;
    public Entity bulletSpawnPointEntity;
    public onShootEvent onShoot;

    public struct onShootEvent
    {
        public bool istriggered;
        public float3 shootFromPosition;
    }
}
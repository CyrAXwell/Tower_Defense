using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ShootAttackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<EntitiesReferences>(out EntitiesReferences entitiesReferences))
        {
            return;
        }
        
        foreach((
            RefRW<LocalTransform> localTransform,
            RefRW<ShootAttack> shootAttack, 
            RefRO<Target> target) 
            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRW<ShootAttack>, 
                RefRO<Target>>())
        {
            if (target.ValueRO.targetEntity == Entity.Null)
                continue;

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            if (math.distance(localTransform.ValueRO.Position, targetLocalTransform.Position) > shootAttack.ValueRO.attackDistance)
                continue;

            ShootVictim targetShootVictim = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.targetEntity);
            float3 targetShootVictimPosition = targetLocalTransform.TransformPoint(targetShootVictim.hitLocalPosition);

            RefRW<LocalTransform> shootAttackGunChassisLocalTransform = 
                SystemAPI.GetComponentRW<LocalTransform>(shootAttack.ValueRO.gunChassisEntity);
            RefRW<LocalToWorld> shootAttackGunChassisLocalToWorld = 
                SystemAPI.GetComponentRW<LocalToWorld>(shootAttack.ValueRO.gunChassisEntity);
            float rotationSpeed = 15;
            float3 aimDirection = targetShootVictimPosition - shootAttackGunChassisLocalToWorld.ValueRO.Position;
            aimDirection = math.normalize(aimDirection);
            quaternion targetRotation = quaternion.LookRotation(aimDirection, math.up());
            shootAttackGunChassisLocalTransform.ValueRW.Rotation = 
                math.slerp(shootAttackGunChassisLocalTransform.ValueRO.Rotation, targetRotation, SystemAPI.Time.DeltaTime * rotationSpeed);

            shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (shootAttack.ValueRW.timer > 0)
                continue;

            shootAttack.ValueRW.timer = shootAttack.ValueRW.timerMax;

            Entity bulletEntity = state.EntityManager.Instantiate(entitiesReferences.bulletPrefabEntity);
            RefRO<LocalToWorld> shootAttackBulletSpawnLocalToWorld = 
                SystemAPI.GetComponentRO<LocalToWorld>(shootAttack.ValueRO.bulletSpawnPointEntity);
            SystemAPI.SetComponent(bulletEntity, LocalTransform.FromPosition(shootAttackBulletSpawnLocalToWorld.ValueRO.Position));
            
            RefRW<LocalTransform> bulletLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(bulletEntity);
            bulletLocalTransform.ValueRW.Rotation = targetRotation;

            RefRW<Bullet> bulletBullet = SystemAPI.GetComponentRW<Bullet>(bulletEntity);
            bulletBullet.ValueRW.damageAmount = shootAttack.ValueRO.damageAmount;
            bulletBullet.ValueRW.speed = shootAttack.ValueRO.bulletSpeed;

            RefRW<Target> bulletTarget = SystemAPI.GetComponentRW<Target>(bulletEntity);
            bulletTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;

            shootAttack.ValueRW.onShoot.istriggered = true;
            shootAttack.ValueRW.onShoot.shootFromPosition = shootAttackBulletSpawnLocalToWorld.ValueRO.Position;

            Entity audioManagerEntity = SystemAPI.GetSingletonEntity<Audio>();
            RefRW<Audio> audio = SystemAPI.GetComponentRW<Audio>(audioManagerEntity);
            audio.ValueRW.shoot = true;
        }
    }
}

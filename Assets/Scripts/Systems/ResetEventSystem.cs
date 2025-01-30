using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
partial struct ResetEventSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<Health> health in SystemAPI.Query<RefRW<Health>>())
        {
            health.ValueRW.onHealthChange = false;
        }

        foreach (RefRW<ShootAttack> shootAttack in SystemAPI.Query<RefRW<ShootAttack>>())
        {
            shootAttack.ValueRW.onShoot.istriggered = false;
        }

        foreach (RefRW<MeleeAttack> meleeAttack in SystemAPI.Query<RefRW<MeleeAttack>>())
        {
            meleeAttack.ValueRW.onAttack = false;
        }

        foreach (RefRW<UnitMover> unitMover in SystemAPI.Query<RefRW<UnitMover>>())
        {
            unitMover.ValueRW.onMoving = false;
        }

        foreach (RefRW<PlayerLevel> playerLevel in SystemAPI.Query<RefRW<PlayerLevel>>())
        {
            playerLevel.ValueRW.onExperiencePointsChange = false;
            playerLevel.ValueRW.onLevelChange.istriggered = false;
            playerLevel.ValueRW.onLevelChange.levelsAmount = 0;
        }

        foreach (RefRW<EnemyAmountCounter> enemyAmountCounter in SystemAPI.Query<RefRW<EnemyAmountCounter>>())
        {
            enemyAmountCounter.ValueRW.destroyedEnemyAmount = 0;
            enemyAmountCounter.ValueRW.spawnedEnemyAmount = 0;
        }
    }
}

using System;
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(HealthDeadTestSystem))]
partial class EnemyAmountCounterSystem : SystemBase
{
    public EventHandler<OnEnemyAmountChangeArgs> OnEnemyAmountChange;
    public class OnEnemyAmountChangeArgs
    {
        public int spawnedAmount;
        public int destroyedAmount;
    }

    protected override void OnUpdate()
    {
        foreach(RefRO<EnemyAmountCounter> enemyAmountCounter in SystemAPI.Query<RefRO<EnemyAmountCounter>>())
        {
            if (enemyAmountCounter.ValueRO.spawnedEnemyAmount != 0 || enemyAmountCounter.ValueRO.destroyedEnemyAmount != 0)
            {
                OnEnemyAmountChange?.Invoke(this, new OnEnemyAmountChangeArgs{ 
                    spawnedAmount = enemyAmountCounter.ValueRO.spawnedEnemyAmount,
                    destroyedAmount = enemyAmountCounter.ValueRO.destroyedEnemyAmount});
            }
        }
    }
}

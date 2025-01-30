using System;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial class PlayerDeadTestSystem : SystemBase
{
    public EventHandler OnPlayerDeath;


    protected override void OnUpdate()
    {
        foreach((
            RefRO<Health> health, 
            RefRO<Unit> unit,
            RefRO<PlayerLevel> playerLevel,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<Health>,
                RefRO<Unit>,
                RefRO<PlayerLevel>>().WithEntityAccess())
        {
            if (health.ValueRO.healthAmount <= 0)
            {
                OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

using System;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(HealthDeadTestSystem))]
partial class LevelUpEventSystem : SystemBase
{
    public EventHandler OnXPChange;
    public EventHandler<int> OnLevelChange;

    protected override void OnUpdate()
    {
        foreach (RefRW<PlayerLevel> playerLevel in SystemAPI.Query<RefRW<PlayerLevel>>())
        {
            if (playerLevel.ValueRO.onLevelChange.istriggered)
            {
                OnLevelChange?.Invoke(this, playerLevel.ValueRO.onLevelChange.levelsAmount);
            }
            
            if (playerLevel.ValueRO.onExperiencePointsChange)
            {
                OnXPChange?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

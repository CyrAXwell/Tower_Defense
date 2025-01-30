using Unity.Burst;
using Unity.Entities;

partial struct LevelUpTestSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<PlayerLevel> playerLevel in SystemAPI.Query<RefRW<PlayerLevel>>())
        {
            while (playerLevel.ValueRO.experiencePoints >= playerLevel.ValueRO.experiencePointsMax)
            {
                playerLevel.ValueRW.level ++;
                playerLevel.ValueRW.experiencePoints -= playerLevel.ValueRO.experiencePointsMax;

                playerLevel.ValueRW.experiencePointsMax = (playerLevel.ValueRO.level + 1) * (playerLevel.ValueRO.level + 1);
                
                playerLevel.ValueRW.onLevelChange.istriggered = true;
                playerLevel.ValueRW.onLevelChange.levelsAmount++;
                playerLevel.ValueRW.onExperiencePointsChange = true;
            }    
        } 
    }
}

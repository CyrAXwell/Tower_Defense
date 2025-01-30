using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public int experiencePointsMax;
    public int level;

    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerLevel{
                experiencePointsMax = authoring.experiencePointsMax,
                level = authoring.level,
                onExperiencePointsChange = true,
                onLevelChange = new PlayerLevel.onLevelChangeEvent{istriggered = true, levelsAmount = 1},
            });
        }
    }
}

public struct PlayerLevel : IComponentData
{
    public int experiencePoints;
    public int experiencePointsMax;
    public int level;   
    public bool onExperiencePointsChange;
    public onLevelChangeEvent onLevelChange;
    public struct onLevelChangeEvent
    {
        public bool istriggered;
        public int levelsAmount;
    }
}

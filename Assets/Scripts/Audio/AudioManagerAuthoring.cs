using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class AudioManagerAuthoring : MonoBehaviour
{
    public class Baker : Baker<AudioManagerAuthoring>
    {
        public override void Bake(AudioManagerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Audio{
            });
        }
    }
}

public struct Audio : IComponentData
{
    public bool shoot;
}

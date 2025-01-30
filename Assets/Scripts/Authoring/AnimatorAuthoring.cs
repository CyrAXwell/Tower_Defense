using Unity.Entities;
using UnityEngine;

public class AnimatorAuthoring : MonoBehaviour
{
    public GameObject unitVisualMeshGameObjectPrefab;

    public class Baker : Baker<AnimatorAuthoring>
    {
        public override void Bake(AnimatorAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new LightMeleeUnitVisualGameObjectPrefab {
                gameObjectPrefab = authoring.unitVisualMeshGameObjectPrefab,
            });
        }
    }
}

public class LightMeleeUnitVisualGameObjectPrefab : IComponentData
{
    public GameObject gameObjectPrefab;
}

public class LightMeleeUnitAnimator : ICleanupComponentData
{
    public Animator animator;
}


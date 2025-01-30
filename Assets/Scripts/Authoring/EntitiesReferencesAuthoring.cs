using Unity.Entities;
using UnityEngine;

public class EntitiesReferencesAuthoring : MonoBehaviour
{
    public GameObject bulletPrefabGameObject;
    public GameObject lightMeleeUnitPrefabGameObject;
    public GameObject shootLightPrefabGameObject;
    public GameObject enemySpawnerPrefabGameObject;
    public GameObject heatAreaSkillGameObject;
    public GameObject freezeAreaSkillGameObject;
    public GameObject poisonAreaSkillGameObject;
    public GameObject lightningSkillGameObject;
    public GameObject meteorSkillGameObject;
    public GameObject healingAreaSkillGameObject;
    public GameObject snowSlashSkillGameObject;

    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences {
                bulletPrefabEntity = GetEntity(authoring.bulletPrefabGameObject, TransformUsageFlags.Dynamic),
                lightMeleeUnitPrefabEntity = GetEntity(authoring.lightMeleeUnitPrefabGameObject, TransformUsageFlags.Dynamic),
                shootLightPrefabEntity = GetEntity(authoring.shootLightPrefabGameObject, TransformUsageFlags.Dynamic),
                heatAreaSkillEntity = GetEntity(authoring.heatAreaSkillGameObject, TransformUsageFlags.Dynamic),
                freezeAreaSkillEntity = GetEntity(authoring.freezeAreaSkillGameObject, TransformUsageFlags.Dynamic),
                poisonAreaSkillEntity = GetEntity(authoring.poisonAreaSkillGameObject, TransformUsageFlags.Dynamic),
                lightningSkillEntity = GetEntity(authoring.lightningSkillGameObject, TransformUsageFlags.Dynamic),
                meteorSkillEntity = GetEntity(authoring.meteorSkillGameObject, TransformUsageFlags.Dynamic),
                healingAreaSkillEntity = GetEntity(authoring.healingAreaSkillGameObject, TransformUsageFlags.Dynamic),
                snowSlashSkillEntity = GetEntity(authoring.snowSlashSkillGameObject, TransformUsageFlags.Dynamic),
                enemySpawnerPrefabEntity = GetEntity(authoring.enemySpawnerPrefabGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct EntitiesReferences : IComponentData
{
    public Entity bulletPrefabEntity;
    public Entity lightMeleeUnitPrefabEntity;
    public Entity shootLightPrefabEntity;
    public Entity enemySpawnerPrefabEntity;
    public Entity heatAreaSkillEntity;
    public Entity freezeAreaSkillEntity;
    public Entity poisonAreaSkillEntity;
    public Entity lightningSkillEntity;
    public Entity meteorSkillEntity;
    public Entity healingAreaSkillEntity;
    public Entity snowSlashSkillEntity;

}
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SnowSlashSkill : ISkill
{
    private SkillSO _skillSO;
    private float _timer;
    private int _level;

    public SnowSlashSkill()
    {
        string path = SkillSOPath.ScriptableObjects + "/" + SkillSOPath.SnowSlashSkillSO;
        _skillSO = Resources.Load<SkillSO>(path);
        _skillSO.Initialize();
    }

    public void Use()
    {
        _timer = _skillSO.Cooldown;

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntitiesReferences entitiesReferences = entityManager.CreateEntityQuery(typeof(EntitiesReferences)).GetSingleton<EntitiesReferences>();
        
        Entity playerEntity = entityManager.CreateEntityQuery(typeof(Friendly)).GetSingletonEntity();
        LocalTransform playerLocalTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);
        Entity snowSlashEntity = entityManager.Instantiate(entitiesReferences.snowSlashSkillEntity);
        LocalTransform snowSlashLocalTransform = entityManager.GetComponentData<LocalTransform>(snowSlashEntity);
        snowSlashLocalTransform.Position = playerLocalTransform.Position;
        entityManager.SetComponentData<LocalTransform>(snowSlashEntity, snowSlashLocalTransform);

        SnowSlash snowSlash = entityManager.GetComponentData<SnowSlash>(snowSlashEntity);
        snowSlash.timer = snowSlash.duration;
        snowSlash = GetUpgrade(snowSlash);
        entityManager.SetComponentData<SnowSlash>(snowSlashEntity, snowSlash);

        GameObject newVisualGameObject = GameObject.Instantiate(_skillSO.VisualGameobject);
        newVisualGameObject.transform.position = snowSlashLocalTransform.Position;
        newVisualGameObject.transform.position += new Vector3(0, 0.5f, 0);
        newVisualGameObject.transform.rotation = snowSlashLocalTransform.Rotation;
        newVisualGameObject.transform.Rotate(0, -45f, 0);

        SnowSlashSkillVisualController visualController = newVisualGameObject.GetComponent<SnowSlashSkillVisualController>();
        visualController.Initialize(snowSlash.size, snowSlash.duration, snowSlash.damageFrequency);
    }

    private SnowSlash GetUpgrade(SnowSlash snowSlash)
    {
        int upgrade = 1;

        while (upgrade < _level)
        {
            UpgradeParameterType type = _skillSO.GetUpgradeType(upgrade);
            switch (type)
            {
                case UpgradeParameterType.Damage :
                    snowSlash.damageAmount += snowSlash.upgradeDamageAmount;
                    break;
            }
            upgrade ++;
        }
        return snowSlash;
    }

    public void UpdateTimer(float deltaTime)
    {
        if (_timer > 0)
        {
            _timer -= deltaTime;
        }
    }

    public bool IsReady()
    {
        return _timer <= 0;
    }

    public float GetTimer()
    {
        return _timer;
    }

    public SkillSO GetSkillSO()
    {
        return _skillSO;
    }

    public void Upgrade()
    {
        _level++;
    }

    public int GetLevel()
    {
        return _level;
    }

    public bool IsMaxLevel()
    {
        return _level == _skillSO.GetUpgradeAmount() + 1;
    }
}

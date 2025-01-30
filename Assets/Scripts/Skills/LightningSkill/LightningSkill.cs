using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class LightningSkill : ISkill
{
    private SkillSO _skillSO;
    private float _timer;
    private int _level;

    public LightningSkill()
    {
        string path = SkillSOPath.ScriptableObjects + "/" + SkillSOPath.LightningSkillSO;
        _skillSO = Resources.Load<SkillSO>(path);
        _skillSO.Initialize();
    }

    public void Use()
    {
        _timer = _skillSO.Cooldown;

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntitiesReferences entitiesReferences = entityManager.CreateEntityQuery(typeof(EntitiesReferences)).GetSingleton<EntitiesReferences>();
        
        Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
        Entity lightningEntity = entityManager.Instantiate(entitiesReferences.lightningSkillEntity);
        LocalTransform lightningLocalTransform = entityManager.GetComponentData<LocalTransform>(lightningEntity);
        lightningLocalTransform.Position = mouseWorldPosition;
        entityManager.SetComponentData<LocalTransform>(lightningEntity, lightningLocalTransform);

        Lightning lightning = entityManager.GetComponentData<Lightning>(lightningEntity);
        lightning.damageDelayTimer = lightning.damageDelay;
        lightning = GetUpgrade(lightning);
        entityManager.SetComponentData<Lightning>(lightningEntity, lightning);

        GameObject newVisualGameObject = GameObject.Instantiate(_skillSO.VisualGameobject);
        newVisualGameObject.transform.position = lightningLocalTransform.Position;
        newVisualGameObject.transform.rotation = lightningLocalTransform.Rotation;

        LightningSkillVisualController visualController = newVisualGameObject.GetComponent<LightningSkillVisualController>();
        visualController.Initialize(lightning.size, lightning.duration);
    }

    private Lightning GetUpgrade(Lightning lightning)
    {
        int upgrade = 1;

        while (upgrade < _level)
        {
            UpgradeParameterType type = _skillSO.GetUpgradeType(upgrade);
            switch (type)
            {
                case UpgradeParameterType.AreaSize :
                    lightning.size += lightning.upgradeSize;
                    break;
                case UpgradeParameterType.Damage :
                    lightning.damageAmount += lightning.upgradeDamageAmount;
                    break;
            }
            upgrade ++;
        }

        return lightning;
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

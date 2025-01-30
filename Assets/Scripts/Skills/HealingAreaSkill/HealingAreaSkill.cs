using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class HealingAreaSkill : ISkill
{
    public const int MAX_SKILL_LEVEL = 6;

    private SkillSO _skillSO;
    private float _timer;
    private int _level;

    public HealingAreaSkill()
    {
        string path = SkillSOPath.ScriptableObjects + "/" + SkillSOPath.HealingAreaSkillSO;
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
        Entity healingAreaEntity = entityManager.Instantiate(entitiesReferences.healingAreaSkillEntity);
        LocalTransform healingAreaLocalTransform = entityManager.GetComponentData<LocalTransform>(healingAreaEntity);
        healingAreaLocalTransform.Position = playerLocalTransform.Position;
        entityManager.SetComponentData<LocalTransform>(healingAreaEntity, healingAreaLocalTransform);

        HealingArea healingArea = entityManager.GetComponentData<HealingArea>(healingAreaEntity);
        healingArea.timer = healingArea.duration;
        healingArea = GetUpgrade(healingArea);
        entityManager.SetComponentData<HealingArea>(healingAreaEntity, healingArea);

        GameObject newVisualGameObject = GameObject.Instantiate(_skillSO.VisualGameobject);
        newVisualGameObject.transform.position = healingAreaLocalTransform.Position;
        newVisualGameObject.transform.rotation = healingAreaLocalTransform.Rotation;

        HealingAreaSkillVisualController visualController = newVisualGameObject.GetComponent<HealingAreaSkillVisualController>();
        visualController.Initialize(healingArea.size, healingArea.duration);
    }

    public void UpdateTimer(float deltaTime)
    {
        if (_timer > 0)
        {
            _timer -= deltaTime;
        }
    }

    private HealingArea GetUpgrade(HealingArea healingArea)
    {
        int upgrade = 1;

        while (upgrade < _level)
        {
            UpgradeParameterType type = _skillSO.GetUpgradeType(upgrade);
            switch (type)
            {
                case UpgradeParameterType.Heal :
                    healingArea.healAmount += healingArea.upgradeHealAmount;
                    break;
            }
            upgrade ++;
        }

        return healingArea;
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

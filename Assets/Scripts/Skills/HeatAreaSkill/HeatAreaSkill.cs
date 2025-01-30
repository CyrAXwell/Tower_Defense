using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class HeatAreaSkill : ISkill
{
    public const int MAX_SKILL_LEVEL = 6;

    private SkillSO _skillSO;
    private float _timer;
    private int _level;

    public HeatAreaSkill()
    {
        string path = SkillSOPath.ScriptableObjects + "/" + SkillSOPath.HeatAreaSkillSO;
        _skillSO = Resources.Load<SkillSO>(path);
        _skillSO.Initialize();
    }

    public void Use()
    {
        _timer = _skillSO.Cooldown;
        
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntitiesReferences entitiesReferences = entityManager.CreateEntityQuery(typeof(EntitiesReferences)).GetSingleton<EntitiesReferences>();
        
        Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
        Entity heatAreaEntity = entityManager.Instantiate(entitiesReferences.heatAreaSkillEntity);
        LocalTransform heatAreaLocalTransform = entityManager.GetComponentData<LocalTransform>(heatAreaEntity);
        heatAreaLocalTransform.Position = mouseWorldPosition;
        entityManager.SetComponentData<LocalTransform>(heatAreaEntity, heatAreaLocalTransform);

        HeatArea heatArea = entityManager.GetComponentData<HeatArea>(heatAreaEntity);
        heatArea.timer = heatArea.duration;
        heatArea = GetUpgrade(heatArea);
        entityManager.SetComponentData<HeatArea>(heatAreaEntity, heatArea);

        GameObject newVisualGameObject = GameObject.Instantiate(_skillSO.VisualGameobject);
        newVisualGameObject.transform.position = heatAreaLocalTransform.Position;
        newVisualGameObject.transform.rotation = heatAreaLocalTransform.Rotation;

        HeatAreaSkillVisualController visualController = newVisualGameObject.GetComponent<HeatAreaSkillVisualController>();
        visualController.Initialize(heatArea.size, heatArea.duration);
    }

    private HeatArea GetUpgrade(HeatArea heatArea)
    {
        int upgrade = 1;

        while (upgrade < _level)
        {
            UpgradeParameterType type = _skillSO.GetUpgradeType(upgrade);
            switch (type)
            {
                case UpgradeParameterType.Damage :
                    heatArea.damageAmount += heatArea.upgradeDamageAmount;
                    break;
                case UpgradeParameterType.AreaSize :
                    heatArea.size += heatArea.upgradeSize;
                    break;
            }
            upgrade ++;
        }

        return heatArea;
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
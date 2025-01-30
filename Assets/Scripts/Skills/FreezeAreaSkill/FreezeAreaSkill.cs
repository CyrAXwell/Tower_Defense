using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FreezeAreaSkill : ISkill
{

    public const int MAX_SKILL_LEVEL = 6;

    private SkillSO _skillSO;
    private float _timer;
    private int _level;

    public FreezeAreaSkill()
    {
        string path = SkillSOPath.ScriptableObjects + "/" + SkillSOPath.FreezeAreaSkillSO;
        _skillSO = Resources.Load<SkillSO>(path);
        _skillSO.Initialize();
    }

    public void Use()
    {
        _timer = _skillSO.Cooldown;

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntitiesReferences entitiesReferences = entityManager.CreateEntityQuery(typeof(EntitiesReferences)).GetSingleton<EntitiesReferences>();
        
        Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
        Entity freezeAreaEntity = entityManager.Instantiate(entitiesReferences.freezeAreaSkillEntity);
        LocalTransform freezeAreaLocalTransform = entityManager.GetComponentData<LocalTransform>(freezeAreaEntity);
        freezeAreaLocalTransform.Position = mouseWorldPosition;
        entityManager.SetComponentData<LocalTransform>(freezeAreaEntity, freezeAreaLocalTransform);

        FreezeArea freezeArea = entityManager.GetComponentData<FreezeArea>(freezeAreaEntity);
        freezeArea.timer = freezeArea.duration;
        freezeArea =  GetUpgrade(freezeArea);
        entityManager.SetComponentData<FreezeArea>(freezeAreaEntity, freezeArea);

        GameObject newVisualGameObject = GameObject.Instantiate(_skillSO.VisualGameobject);
        newVisualGameObject.transform.position = freezeAreaLocalTransform.Position;
        newVisualGameObject.transform.rotation = freezeAreaLocalTransform.Rotation;

        FreezeAreaSkillVisualController visualController = newVisualGameObject.GetComponent<FreezeAreaSkillVisualController>();
        visualController.Initialize(freezeArea.size, freezeArea.duration);
    }

    private FreezeArea GetUpgrade(FreezeArea freezeArea)
    {
        int upgrade = 1;

        while (upgrade < _level)
        {
            UpgradeParameterType type = _skillSO.GetUpgradeType(upgrade);
            switch (type)
            {
                case UpgradeParameterType.AreaSize :
                    freezeArea.size += freezeArea.upgradeSize;
                    break;
                case UpgradeParameterType.Freeze :
                    freezeArea.freezeMultiplayer += freezeArea.upgradefreezeMultiplayer;
                    break;
                case UpgradeParameterType.Damage :
                    freezeArea.damageAmount += freezeArea.upgradeDamageAmount;
                    break;
            }
            upgrade ++;
        }

        return freezeArea;
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

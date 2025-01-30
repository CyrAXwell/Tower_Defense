using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PoisonAreaSkill : ISkill
{
    private SkillSO _skillSO;
    private float _timer;
    private int _level;

    public PoisonAreaSkill()
    {
        string path = SkillSOPath.ScriptableObjects + "/" + SkillSOPath.PoisonAreaSkillSO;
        _skillSO = Resources.Load<SkillSO>(path);
        _skillSO.Initialize();
    }

    public void Use()
    {
        _timer = _skillSO.Cooldown;;

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntitiesReferences entitiesReferences = entityManager.CreateEntityQuery(typeof(EntitiesReferences)).GetSingleton<EntitiesReferences>();
        
        Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
        Entity poisonAreaEntity = entityManager.Instantiate(entitiesReferences.poisonAreaSkillEntity);
        LocalTransform poisonAreaLocalTransform = entityManager.GetComponentData<LocalTransform>(poisonAreaEntity);
        poisonAreaLocalTransform.Position = mouseWorldPosition;
        entityManager.SetComponentData<LocalTransform>(poisonAreaEntity, poisonAreaLocalTransform);

        PoisonArea poisonArea = entityManager.GetComponentData<PoisonArea>(poisonAreaEntity);
        poisonArea.timer = poisonArea.duration;
        poisonArea = GetUpgrade(poisonArea);
        entityManager.SetComponentData<PoisonArea>(poisonAreaEntity, GetUpgrade(poisonArea));

        GameObject newPoisonAreaVisualiseGameObject = GameObject.Instantiate(_skillSO.VisualGameobject);
        newPoisonAreaVisualiseGameObject.transform.position = poisonAreaLocalTransform.Position + new float3(0, 0.01f, 0);
        newPoisonAreaVisualiseGameObject.transform.rotation = poisonAreaLocalTransform.Rotation;

        PoisonAreaSkillVisualController visualController = newPoisonAreaVisualiseGameObject.GetComponent<PoisonAreaSkillVisualController>();
        visualController.Initialize(poisonArea.size, poisonArea.duration);
    }

    private PoisonArea GetUpgrade(PoisonArea poisonArea)
    {
        int upgrade = 1;

        while (upgrade < _level)
        {
            UpgradeParameterType type = _skillSO.GetUpgradeType(upgrade);
            switch (type)
            {
                case UpgradeParameterType.AreaSize :
                    poisonArea.size += poisonArea.upgradeSize;
                    break;
                case UpgradeParameterType.Damage :
                    poisonArea.damageAmount += poisonArea.upgradeDamageAmount;
                    break;
            }
            upgrade ++;
        }
        return poisonArea;
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

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class MeteorSkill : ISkill
{
    private SkillSO _skillSO;
    private float _timer;
    private int _level;

    public MeteorSkill()
    {
        string path = SkillSOPath.ScriptableObjects + "/" + SkillSOPath.MeteorSkillSO;
        _skillSO = Resources.Load<SkillSO>(path);
        _skillSO.Initialize();
    }

    public void Use()
    {
        _timer = _skillSO.Cooldown;

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntitiesReferences entitiesReferences = entityManager.CreateEntityQuery(typeof(EntitiesReferences)).GetSingleton<EntitiesReferences>();
        
        Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
        Entity meteorEntity = entityManager.Instantiate(entitiesReferences.meteorSkillEntity);
        LocalTransform meteorLocalTransform = entityManager.GetComponentData<LocalTransform>(meteorEntity);
        meteorLocalTransform.Position = mouseWorldPosition;
        entityManager.SetComponentData<LocalTransform>(meteorEntity, meteorLocalTransform);

        Meteor meteor = entityManager.GetComponentData<Meteor>(meteorEntity);
        meteor.damageDelayTimer = meteor.damageDelay;
        meteor = GetUpgrade(meteor);
        entityManager.SetComponentData<Meteor>(meteorEntity, meteor);

        GameObject newVisualGameObject = GameObject.Instantiate(_skillSO.VisualGameobject);
        newVisualGameObject.transform.position = meteorLocalTransform.Position;
        newVisualGameObject.transform.rotation = meteorLocalTransform.Rotation;

        MeteorSkillVisualController visualController = newVisualGameObject.GetComponent<MeteorSkillVisualController>();
        visualController.Initialize(meteor.size, meteor.duration);
    }

    private Meteor GetUpgrade(Meteor meteor)
    {
        int upgrade = 1;

        while (upgrade < _level)
        {
            UpgradeParameterType type = _skillSO.GetUpgradeType(upgrade);
            switch (type)
            {
                case UpgradeParameterType.AreaSize :
                    meteor.size += meteor.upgradeSize;
                    break;
                case UpgradeParameterType.Damage :
                    meteor.damageAmount += meteor.upgradeDamageAmount;
                    break;
            }
            upgrade ++;
        }
        return meteor;
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

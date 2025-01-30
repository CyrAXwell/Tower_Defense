using Unity.Entities;
using UnityEngine;

public class PlayerUpgrade
{
    private int _level;
    private PlayerUpgradeSO _playerUpgradeSO;
    
    public PlayerUpgrade()
    {
        string path = SkillSOPath.ScriptableObjects + "/" + SkillSOPath.PlayerUpgradeSO;
        _playerUpgradeSO = Resources.Load<PlayerUpgradeSO>(path);
        _playerUpgradeSO.Initialize();
    }

    public PlayerUpgradeSO GetPlayerUpgradeSO()
    {
        return  _playerUpgradeSO;
    }

    public int GetLevel()
    {
        return _level;
    }

    public void Upgrade()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity playerEntity = entityManager.CreateEntityQuery(typeof(Friendly)).GetSingletonEntity();

        UpgradeParameterType type = _playerUpgradeSO.GetUpgradeType(_level);
        ShootAttack shootAttack = entityManager.GetComponentData<ShootAttack>(playerEntity);
        switch (type)
        {
            case UpgradeParameterType.PlayerDamage : 
                shootAttack.damageAmount += _playerUpgradeSO.UpgradeBulletDamage;
                entityManager.SetComponentData<ShootAttack>(playerEntity, shootAttack);
                break;
            case UpgradeParameterType.PlayerHealth : 
                Health health = entityManager.GetComponentData<Health>(playerEntity);
                health.healthAmountMax += _playerUpgradeSO.UpgradeHealth;
                health.healthAmount += _playerUpgradeSO.UpgradeHealth;
                entityManager.SetComponentData<Health>(playerEntity, health);
                break;
            case UpgradeParameterType.PlayerFireRate : 
                shootAttack.timerMax += _playerUpgradeSO.UpgradeFireRate;
                shootAttack.bulletSpeed += _playerUpgradeSO.UpgradeBulletSpeed;
                entityManager.SetComponentData<ShootAttack>(playerEntity, shootAttack);
                break;
        }
        _level++;
    }
}

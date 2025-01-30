using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgradeSO", menuName = "ScriptableObject/PlayerUpgrade")]
public class PlayerUpgradeSO : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Color _iconColor;
    [SerializeField] private Color _backgroundColor;
    [SerializeField] private int _upgradeBulletDamage;
    [SerializeField] private int _upgradeHealth;
    [SerializeField] private int _upgradeBulletSpeed;
    [SerializeField] private float _upgradeFireRate;
    [SerializeField] private List<UpgradeInfo> _upgradeDecriptionList;
    [SerializeField] private List<UpgradeParameterType> _upgradeParameterTypeList;

    private Dictionary<UpgradeParameterType, string> _upgradesDecriptionsDictionary;

    public Sprite Icon => _icon;
    public Color IconColor => _iconColor;
    public Color BackgroundColor => _backgroundColor;
    public int UpgradeBulletDamage => _upgradeBulletDamage;
    public int UpgradeHealth => _upgradeHealth;
    public int UpgradeBulletSpeed => _upgradeBulletSpeed;
    public float UpgradeFireRate => _upgradeFireRate;

    public void Initialize()
    {
        _upgradesDecriptionsDictionary = new Dictionary<UpgradeParameterType, string>();
        foreach (UpgradeInfo upgrade in _upgradeDecriptionList)
        {
            _upgradesDecriptionsDictionary.Add(upgrade.Type, upgrade.Description);
        }
    }

    public string GetDescription(int upgradeLevel)
    {
        UpgradeParameterType upgradeParameterTypetype = _upgradeParameterTypeList[upgradeLevel];
        if (_upgradesDecriptionsDictionary.ContainsKey(upgradeParameterTypetype))
            return _upgradesDecriptionsDictionary[upgradeParameterTypetype];
        else
            return "";
    }

    public UpgradeParameterType GetUpgradeType(int level)
    {
        if (level < _upgradeParameterTypeList.Count)
            return _upgradeParameterTypeList[level];
        else
            return UpgradeParameterType.Default;
    }
}

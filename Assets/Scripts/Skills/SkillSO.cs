using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "ScriptableObject/Skills")]
public class SkillSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private float _cooldown;
    [SerializeField] private GameObject _visualGameobject;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Color _iconColor;
    [SerializeField] private Color _backgroundColor;
    [SerializeField, TextArea] private string _description;
    [SerializeField] private List<UpgradeInfo> _upgradeDecriptionList;
    [SerializeField] private List<UpgradeParameterType> _upgradeParameterTypeList;

    private Dictionary<UpgradeParameterType, string> _upgradesDecriptionsDictionary;

    public string Name => _name;
    public float Cooldown => _cooldown;
    public GameObject VisualGameobject => _visualGameobject;
    public Sprite Icon => _icon;
    public Color IconColor => _iconColor;
    public Color BackgroundColor => _backgroundColor;
    public string Description => _description;

    public void Initialize()
    {
        _upgradesDecriptionsDictionary = new Dictionary<UpgradeParameterType, string>();
        foreach (UpgradeInfo upgrade in _upgradeDecriptionList)
        {
            _upgradesDecriptionsDictionary.Add(upgrade.Type, upgrade.Description);
        }
    }

    public UpgradeParameterType GetUpgradeType(int upgradeLevel)
    {
        if (upgradeLevel < _upgradeParameterTypeList.Count)
            return _upgradeParameterTypeList[upgradeLevel];
        else
            return UpgradeParameterType.Default;
    }

    public string GetDescription(int upgradeLevel)
    {
        UpgradeParameterType upgradeParameterTypetype = _upgradeParameterTypeList[upgradeLevel - 1];
        if (_upgradesDecriptionsDictionary.ContainsKey(upgradeParameterTypetype))
            return _upgradesDecriptionsDictionary[upgradeParameterTypetype];
        else
            return "";
    }

    public int GetUpgradeAmount()
    {
        return _upgradeParameterTypeList.Count;
    }
}

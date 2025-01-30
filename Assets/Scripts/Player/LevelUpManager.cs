using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public const int DISPLAY_UPGRADES_AMOUNT = 3;
    
    [SerializeField] private LevelUpMenuUI _levelUpMenuUI;

    private Dictionary<UpgradeType, ISkill> _skillsDictionary;
    private List<UpgradeType> _upgradeTypeList;
    private List<UpgradeType> _activeUpgradeList;
    private PlayerUpgrade _playerUpgrade;
    private bool _isGameStart = true;
    private LevelUpEventSystem _levelUpEventSystem;
    private int _levelsUpAmount;

    private void Start()
    {
        _skillsDictionary = new Dictionary<UpgradeType, ISkill>();
        _upgradeTypeList = new List<UpgradeType>();
        _activeUpgradeList= new List<UpgradeType>();
        _playerUpgrade = new PlayerUpgrade();

        foreach (UpgradeType type in (UpgradeType[]) Enum.GetValues(typeof(UpgradeType)))
        {
            _skillsDictionary.Add(type, GetUpgrade(type));
            _upgradeTypeList.Add(type);
        }

        _isGameStart = true;

        _levelUpEventSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<LevelUpEventSystem>();
        _levelUpEventSystem.OnLevelChange += OnLevelChange;
        _levelUpMenuUI.GetRefuseButton().onClick.AddListener(() => {OnRefuseButtonClick();});
    }

    private void OnRefuseButtonClick()
    {
        SkillUpgradeButtonUI[] buttons = _levelUpMenuUI.GetButtons();
        foreach(SkillUpgradeButtonUI upgradeButton in buttons)
        {
            upgradeButton.OnClick -= OnSelectUpgrade;
        }

        _levelUpMenuUI.ShowPlayerUpgrade(_playerUpgrade);
        _levelUpMenuUI.GetPlayerUpgradeButton().OnClick += OnUpgradePlayer;
    }

    private void OnLevelChange(object sender, int e)
    {
        _levelsUpAmount = e;
        GetLevelUpReward();
    }

    private void GetLevelUpReward()
    {
        _levelsUpAmount--;
        if (_isGameStart)
        {
            _isGameStart = false;
            return;
        }
        GameManager.Instance.PasueGame();
        Dictionary<UpgradeType, ISkill> upgrades = GetRandomUpgrades();

        _levelUpMenuUI.ShowLevelUpMenu(upgrades);

        SkillUpgradeButtonUI[] buttons = _levelUpMenuUI.GetButtons();
        foreach(SkillUpgradeButtonUI button in buttons)
        {
            button.OnClick += OnSelectUpgrade;
        }
        if (upgrades.Count == 0)
        {
            OnRefuseButtonClick();
        }
    }

    private void OnUpgradePlayer(object sender, EventArgs e)
    {
        _levelUpMenuUI.GetPlayerUpgradeButton().OnClick -= OnUpgradePlayer;

        _levelUpMenuUI.HideLevelUpMenu();
        _playerUpgrade.Upgrade();
        GameManager.Instance.UnpasueGame();

        levelsUpAmountChack();
    }

    private void OnSelectUpgrade(object sender, EventArgs e)
    {
        SkillUpgradeButtonUI[] buttons = _levelUpMenuUI.GetButtons();
        foreach(SkillUpgradeButtonUI UpgradeButton in buttons)
        {
            UpgradeButton.OnClick -= OnSelectUpgrade;
        }

        _levelUpMenuUI.HideLevelUpMenu();
        SkillUpgradeButtonUI button = sender as SkillUpgradeButtonUI;
        
        ISkill skill = button.GetSkill();
        if (skill.GetLevel() == 0)
        {
            SkillManager.Instance.AddNewSkill(skill);
            SkillManager.Instance.UpgradeSkill(skill);

            UpgradeType upgradeType = _skillsDictionary.FirstOrDefault(skills => skills.Value == skill).Key;
            _activeUpgradeList.Add(upgradeType);

            if (_activeUpgradeList.Count == SkillManager.MAX_SKILL_AMOUNT)
            {
                _upgradeTypeList = _activeUpgradeList;
                Dictionary<UpgradeType, ISkill> newSkillsDictionary = new Dictionary<UpgradeType, ISkill>();
                foreach (UpgradeType type in _upgradeTypeList)
                {   
                    newSkillsDictionary.Add(type ,_skillsDictionary[type]);
                }

                _skillsDictionary = newSkillsDictionary;
            }
        }
        else
        {
            SkillManager.Instance.UpgradeSkill(skill);
        }

        if (skill.IsMaxLevel())
        {
            _skillsDictionary.Remove(button.GetSkillType());
            _upgradeTypeList.Remove(button.GetSkillType());
            _activeUpgradeList.Remove(button.GetSkillType());
        }
        GameManager.Instance.UnpasueGame();

        levelsUpAmountChack();
    }

    private void levelsUpAmountChack()
    {
        if (_levelsUpAmount > 0)
        {
            GetLevelUpReward();
        }
    }

    private Dictionary<UpgradeType, ISkill> GetRandomUpgrades()
    {
        Dictionary<UpgradeType, ISkill> copySkillsDictionary = _skillsDictionary.ToDictionary(entry => entry.Key, entry => entry.Value);
        List<UpgradeType> copyUpgradeTypeList = _upgradeTypeList.ToList();

        int upgradeAmount = _skillsDictionary.Count > DISPLAY_UPGRADES_AMOUNT ? DISPLAY_UPGRADES_AMOUNT : _skillsDictionary.Count;
        Dictionary<UpgradeType, ISkill> randomUpgrades = new Dictionary<UpgradeType, ISkill>();
        for (int i = 0; i < upgradeAmount; i++)
        {
            UpgradeType upgrade = copyUpgradeTypeList[UnityEngine.Random.Range(0, copyUpgradeTypeList.Count)];

            randomUpgrades.Add(upgrade, copySkillsDictionary[upgrade]);
            copySkillsDictionary.Remove(upgrade);
            copyUpgradeTypeList.Remove(upgrade);
        }

        return randomUpgrades;
    }

    private ISkill GetUpgrade(UpgradeType type)
    {
        ISkill newSkill = null;
        switch (type)
        {
            case UpgradeType.FreezeAreaSkill :
                newSkill = new FreezeAreaSkill();
                break;
            case UpgradeType.HealingAreaSkill :
                newSkill = new HealingAreaSkill();
                break;
            case UpgradeType.HeatAreaSkill :
                newSkill = new HeatAreaSkill();
                break;
            case UpgradeType.LightningSkill :
                newSkill = new LightningSkill();
                break;
            case UpgradeType.MeteorSkill :
                newSkill = new MeteorSkill();
                break;
            case UpgradeType.PoisonAreaSkill :
                newSkill = new PoisonAreaSkill();
                break;
            case UpgradeType.SnowSlashSkill :
                newSkill = new SnowSlashSkill();
                break;
        }
        return newSkill;
    }

    private void OnDisable()
    {
        _levelUpEventSystem.OnLevelChange -= OnLevelChange;
    }
}

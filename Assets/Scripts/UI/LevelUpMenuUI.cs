using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _levelUpMenuPanel;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private SkillUpgradeButtonUI _skillUpgradeButtonUIPrefab;
    [SerializeField] private PlayerUpgradeButtonUI _playerUpgradeButtonUIPrefab;
    [SerializeField] private Button _refuseButton;

    private SkillUpgradeButtonUI[] _buttons;
    private PlayerUpgradeButtonUI _playerUpgradeButton;

    public void ShowLevelUpMenu(Dictionary<UpgradeType, ISkill> skillDictionary)
    {
        _refuseButton.gameObject.SetActive(true);
        if (_buttons != null)
            ClearButtons();

        _levelUpMenuPanel.SetActive(true);
        _buttons = new SkillUpgradeButtonUI[skillDictionary.Count()];

        int i = 0;
        foreach (var skill in skillDictionary)
        {
            SkillUpgradeButtonUI newUpgrade = 
                Instantiate(_skillUpgradeButtonUIPrefab.gameObject, _parentTransform).GetComponent<SkillUpgradeButtonUI>();

            newUpgrade.Initialize(skill.Value, skill.Key);
            _buttons[i] = newUpgrade;
            i++;
        }
    }

    public void ShowPlayerUpgrade(PlayerUpgrade playerUpgrade)
    {
        _refuseButton.gameObject.SetActive(false);

        if (_buttons != null)
            ClearButtons();
        PlayerUpgradeButtonUI newUpgradeButton = 
            Instantiate(_playerUpgradeButtonUIPrefab.gameObject, _parentTransform).GetComponent<PlayerUpgradeButtonUI>();
        
        _playerUpgradeButton = newUpgradeButton;
        newUpgradeButton.Initialize(playerUpgrade);
    }

    private void ClearButtons()
    {
        foreach (SkillUpgradeButtonUI button in _buttons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }

        if (_playerUpgradeButton != null)
            Destroy(_playerUpgradeButton.gameObject);
    }

    public void HideLevelUpMenu()
    {
        _levelUpMenuPanel.SetActive(false);
    }

    public Button GetRefuseButton()
    {
        return _refuseButton;
    }

    public SkillUpgradeButtonUI[] GetButtons()
    {
        return _buttons;
    }

    public PlayerUpgradeButtonUI GetPlayerUpgradeButton()
    {
        return _playerUpgradeButton;
    }
}

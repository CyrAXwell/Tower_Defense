using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillManagerUI : MonoBehaviour
{
    [SerializeField] private SkillButtonUI _skillButtonUIPrefab;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private TMP_Text _useSkillTooltip;
    
    private List<SkillButtonUI> _skillButtonList;
    private SkillManager _skillManager;

    private bool _isFirstSkill = true;
    
    private void Start()
    {
        _useSkillTooltip.gameObject.SetActive(false);
        _skillButtonList = new List<SkillButtonUI>();
        
        _skillManager = SkillManager.Instance;
        _skillManager.OnAddNewSkill += OnAddNewSkill;
        _skillManager.OnUpdateTimer += OnUpdateTimer;
        _skillManager.OnUseSkill += OnUseSkill;
        _skillManager.OnSelectSkill += OnSelectSkill;
        _skillManager.OnUpgradeSkill += OnUpgradeSkill;
    }

    private void OnAddNewSkill(object sender, bool isReady)
    {
        AddNewSkillButton(isReady);

        if (_isFirstSkill)
        {
            _isFirstSkill = false;
            _useSkillTooltip.gameObject.SetActive(true);
        }
    }

    private void OnUpgradeSkill(object sender, SkillManager.UpgradeSkillEventArgs upgrade)
    {
        _skillButtonList[upgrade.Index].UpdateSkillLevel(upgrade.Level, upgrade.IsMaxLevel);
    }

    private void OnSelectSkill(object sender, int index)
    {
        if (index > _skillButtonList.Count - 1)
            return;
        
        foreach (SkillButtonUI skillButton in _skillButtonList)
        {
            skillButton.Deselect();
        }
        _skillButtonList[index].Select();
    }

    private void OnUseSkill(object sender, int index)
    {
        _skillButtonList[index].ShowTimer();
    }

    private void OnUpdateTimer(object sender, EventArgs e)
    {
        foreach (SkillButtonUI skillButton in _skillButtonList)
        {
            if (!skillButton.IsReady)
            {
                if(_skillManager.IsSkillReady(skillButton.GetIndex))
                {
                    skillButton.HideTimer();
                }
                else
                {
                    skillButton.UpdateTimer(_skillManager.GetSkillTimer(skillButton.GetIndex));
                }
            }
        }
    }

    public void AddNewSkillButton(bool isReady)
    {
        SkillButtonUI newSkillButtonUI = Instantiate(_skillButtonUIPrefab.gameObject, _parentTransform).GetComponent<SkillButtonUI>();
        newSkillButtonUI.Initialize(_skillButtonList.Count, isReady, _skillManager.GetSkillSO(_skillButtonList.Count));

        _skillButtonList.Add(newSkillButtonUI);
    }
}

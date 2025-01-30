using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUpgradeButtonUI : MonoBehaviour
{
    public EventHandler OnClick;

    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _level;

    private ISkill _skill;
    private UpgradeType _type;

    public void Initialize(ISkill skill, UpgradeType type)
    {
        _skill = skill;
        _type = type;

        SkillSO skillSO = _skill.GetSkillSO();

        _icon.sprite = skillSO.Icon;
        _background.color = skillSO.IconColor;
        _name.text = skillSO.Name.ToUpper();

        if (skill.GetLevel() == 0)
        {
            _title.text = "NEW SKILL";
            _description.text = skillSO.Description;
        }
        else
        {
            _title.text = "SKILL UPGRADE";
           _description.text = skillSO.GetDescription(skill.GetLevel()); 
        }

        _level.text = "LEVEL " + (skill.GetLevel() + 1).ToString();

        _button.onClick.AddListener(() => { OnClick?.Invoke(this, EventArgs.Empty); } );
    }

    public ISkill GetSkill()
    {
        return _skill;
    }

    public UpgradeType GetSkillType()
    {
        return _type;
    }
}

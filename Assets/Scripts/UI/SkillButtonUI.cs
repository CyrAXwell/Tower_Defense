using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private Image _selected;
    [SerializeField] private TMP_Text _indexText;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private Image _timerPanel;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _level;

    private int _index;
    private bool _isSelected;
    private bool _isReady;

    public int GetIndex => _index;
    public bool IsSelected => _isSelected;
    public bool IsReady => _isReady;

    public void Initialize(int index, bool isReady, SkillSO skillSO)
    {
        _index = index;
        _indexText.text = (_index + 1).ToString();
        _isReady = isReady;
        _timerPanel.gameObject.SetActive(false);
        Deselect();

        _icon.sprite = skillSO.Icon;
        _background.color = skillSO.IconColor;
        _level.text = "LV 1";
    }

    public void Select()
    {
        _isSelected = true;
        _selected.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        _selected.gameObject.SetActive(false);
        _isSelected = false;
    }

    public void UpdateTimer(float time)
    {
        _timerText.text = time.ToString("0.#");
    }

    public void ShowTimer()
    {
        _timerPanel.gameObject.SetActive(true);
        _timerText.text = "";
        _isReady = false;
    }

    public void HideTimer()
    {
        _timerPanel.gameObject.SetActive(false);
        _isReady = true;
    }

    public void UpdateSkillLevel(int level, bool isMaxLevel)
    {
        if (!isMaxLevel)
            _level.text = "LV " + level;
        else
            _level.text = "LV " + level + " MAX";
    }
}

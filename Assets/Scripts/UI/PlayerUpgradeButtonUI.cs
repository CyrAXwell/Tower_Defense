using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeButtonUI : MonoBehaviour
{
    public EventHandler OnClick;
    
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _background;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _level;
    
    public void Initialize(PlayerUpgrade playerUpgrade)
    {
        PlayerUpgradeSO playerUpgradeSO = playerUpgrade.GetPlayerUpgradeSO();
        
        _title.text = "PLAYER UPGRADE";
        _background.color = playerUpgradeSO.IconColor;
        _level.text = "LEVEL " + (playerUpgrade.GetLevel() + 1).ToString();
        _description.text = playerUpgradeSO.GetDescription(playerUpgrade.GetLevel());

        _button.onClick.AddListener(() => { OnClick?.Invoke(this, EventArgs.Empty); } );
    }
}

using System;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Image _xpBar;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _xpText;

    private LevelUpEventSystem _levelUpEventSystem;

    private void Start()
    {
        _levelUpEventSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<LevelUpEventSystem>();
        _levelUpEventSystem.OnLevelChange += OnLevelChange;
        _levelUpEventSystem.OnXPChange += OnXPChange;
    }

    private void OnXPChange(object sender, EventArgs e)
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Entity playerEntity = entityManager.CreateEntityQuery(typeof(PlayerLevel)).GetSingletonEntity();
        PlayerLevel playerLevel = entityManager.GetComponentData<PlayerLevel>(playerEntity);

        _xpBar.transform.localScale = new Vector3((float)playerLevel.experiencePoints / playerLevel.experiencePointsMax, 1, 1);
        _xpText.text = "EXP: " + playerLevel.experiencePoints + "/" + playerLevel.experiencePointsMax;
    }

    private void OnLevelChange(object sender, int e)
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Entity playerEntity = entityManager.CreateEntityQuery(typeof(PlayerLevel)).GetSingletonEntity();
        PlayerLevel playerLevel = entityManager.GetComponentData<PlayerLevel>(playerEntity);

        _levelText.text = "Level: " + playerLevel.level;
    }

    private void OnDisable()
    {
        _levelUpEventSystem.OnLevelChange -= OnLevelChange;
        _levelUpEventSystem.OnXPChange -= OnXPChange;
    }
}

using TMPro;
using Unity.Entities;
using UnityEngine;

public class EnemyAmountCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _enemyCounterText;
    [SerializeField] private TMP_Text _kilsCounterText;

    private int _enemyCounter;
    private int _killsCounter;
    private int _spawnedEnemies;
    private EnemyAmountCounterSystem _enemyAmountCounterSystem;

    private void Start()
    {
        _enemyCounter = 0;
        _killsCounter = 0;
        _spawnedEnemies = 0;

        UpdateCounter(_spawnedEnemies, _killsCounter);
        _enemyAmountCounterSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EnemyAmountCounterSystem>();
        _enemyAmountCounterSystem.OnEnemyAmountChange += OnEnemyAmountChange;
    }

    public int GetDestroyedEnemiesAmount() => _killsCounter;
    public int GetSpawnedEnemiesAmount() => _spawnedEnemies;

    private void OnEnemyAmountChange(object sender, EnemyAmountCounterSystem.OnEnemyAmountChangeArgs e)
    {
        UpdateCounter(e.spawnedAmount, e.destroyedAmount);
    }

    private void UpdateCounter(int spawnedAmount, int destroyedAmount)
    {
        _enemyCounter += spawnedAmount - destroyedAmount;
        _killsCounter += destroyedAmount;
        _spawnedEnemies += spawnedAmount;

        _enemyCounterText.text = "Enemies: " + _enemyCounter.ToString();
        _kilsCounterText.text = "Kills: " + _killsCounter.ToString();
    }
}
